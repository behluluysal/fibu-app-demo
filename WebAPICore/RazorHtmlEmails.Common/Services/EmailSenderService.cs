using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using RazorHtmlEmails.Common.Models.EmailSender;
using DataStore.EF.Data;
using Core.Models.EmailSender;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using RazorHtmlEmails.RazorClassLib.Services;
using RazorHtmlEmails.RazorClassLib.Views.Emails.NewAccount;
using RazorHtmlEmails.RazorClassLib.Models;
using Microsoft.Extensions.DependencyInjection;

namespace RazorHtmlEmails.Common.Services
{
    public class EmailSenderService
    {
        public class EmailSenderHostedService : IEmailSender, IHostedService, IDisposable
        {
            private readonly BufferBlock<MimeMessage> mailMessages;
            private readonly ILogger logger;
            private readonly IRazorViewToStringRenderer razorViewToStringRenderer;
            private readonly IOptionsMonitor<SmtpOptions> optionsMonitor;
            private CancellationTokenSource deliveryCancellationTokenSource;
            private Task deliveryTask;
            private readonly IServiceScopeFactory _scopeFactory;

            public EmailSenderHostedService(IConfiguration configuration, 
                IOptionsMonitor<SmtpOptions> optionsMonitor,
                ILogger<EmailSenderHostedService> logger,
                IServiceScopeFactory scopeFactory)
            {
                this.optionsMonitor = optionsMonitor;
                this.logger = logger;
                this.mailMessages = new BufferBlock<MimeMessage>();
                _scopeFactory = scopeFactory;
                _scopeFactory = scopeFactory;
            }

            public async Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                Email newEmail;
                int affectedRows;

                using (var scope = _scopeFactory.CreateScope())
                {
                    var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    newEmail = new Email { EmailTo = email, Subject = subject, Message = htmlMessage };
                    _db.Emails.Add(newEmail);
                    affectedRows = await _db.SaveChangesAsync();
                    await _db.DisposeAsync();
                }
               

                var message = CreateMessage(email, subject, htmlMessage);
                message.MessageId = newEmail.Id.ToString();


                if (affectedRows != 1)
                {
                    throw new InvalidOperationException($"Could not persist email message to {email}");
                }

                await this.mailMessages.SendAsync(message);
            }

            public async Task StartAsync(CancellationToken token)
            {
                logger.LogInformation("Starting background e-mail delivery");

                IEnumerable<Email> emails;

                using (var scope = _scopeFactory.CreateScope())
                {
                    var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    emails = _db.Emails.Where(x => x.Status != Email.MailStatus.Sent && x.Status != Email.MailStatus.Deleted).ToList();
                    await _db.DisposeAsync();
                }

                try
                {
                    foreach (var item in emails)
                    {
                        var message = CreateMessage(item.EmailTo, item.Subject, item.Message, item.Id.ToString());

                        await this.mailMessages.SendAsync(message, token);
                    }

                    logger.LogInformation("Email delivery started: {count} message(s) were resumed for delivery", emails.Count());

                    deliveryCancellationTokenSource = new CancellationTokenSource();
                    deliveryTask = DeliverAsync(deliveryCancellationTokenSource.Token);
                }
                catch (Exception startException)
                {
                    logger.LogError(startException, "Couldn't start email delivery");
                }
            }

            public async Task StopAsync(CancellationToken token)
            {
                CancelDeliveryTask();
                // Wait for the send task to stop gracefully. If it takes too much, then we stop waiting
                // as soon as the application cancels the token (i.e when it signals it's not willing to wait any longer)
                await Task.WhenAny(deliveryTask, Task.Delay(Timeout.Infinite, token));
            }

            private void CancelDeliveryTask()
            {
                try
                {
                    if (deliveryCancellationTokenSource != null)
                    {
                        logger.LogInformation("Stopping e-mail background delivery");
                        deliveryCancellationTokenSource.Cancel();
                        deliveryCancellationTokenSource = null;
                    }
                }
                catch
                {
                }
            }

            public async Task DeliverAsync(CancellationToken token)
            {
                logger.LogInformation("E-mail background delivery started");
                while (!token.IsCancellationRequested)
                {
                    MimeMessage message = null;
                    Email sentEmail;
                    try
                    {
                        message = await mailMessages.ReceiveAsync(token);

                        var options = this.optionsMonitor.CurrentValue;
                        using var client = new SmtpClient();

                        await client.ConnectAsync(options.Host, options.Port, options.Security, token);
                        if (!string.IsNullOrEmpty(options.Username))
                        {
                            await client.AuthenticateAsync(options.Username, options.Password, token);
                        }

                        await client.SendAsync(message, token);
                        await client.DisconnectAsync(true, token);

                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                            sentEmail = _db.Emails.FirstOrDefault(x => x.Id == int.Parse(message.MessageId));
                            sentEmail.Status = Email.MailStatus.Sent;
                            _db.Update(sentEmail);
                            await _db.SaveChangesAsync();
                            await _db.DisposeAsync();
                        }

                        
                        logger.LogInformation($"E-mail sent successfully to {message.To}");
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception sendException)
                    {
                        var recipient = message?.To[0];
                        logger.LogError(sendException, "Couldn't send an e-mail to {recipient}", recipient);

                        // Increment the sender count
                        try
                        {
                            Email email;

                            using (var scope = _scopeFactory.CreateScope())
                            {
                                var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                                email = _db.Emails.FirstOrDefault(x => x.Id == int.Parse(message.MessageId));
                                await _db.DisposeAsync();
                            }


                            bool shouldRequeue = false;
                            if ((email.Status != Email.MailStatus.Sent && email.Status != Email.MailStatus.Deleted) && email.Tries < 10)
                                shouldRequeue = true;

                            if (shouldRequeue)
                            {
                                await mailMessages.SendAsync(message, token);
                            }
                        }
                        catch (Exception requeueException)
                        {
                            logger.LogError(requeueException, "Couldn't requeue message to {0}", recipient);
                        }

                        // An unexpected error occurred during delivery, so we wait before moving on
                        await Task.Delay(optionsMonitor.CurrentValue.DelayOnError, token);
                    }
                }

                logger.LogInformation("E-mail background delivery stopped");
            }

            public void Dispose()
            {
                CancelDeliveryTask();
            }

            private MimeMessage CreateMessage(string email, string subject, string htmlMessage, string messageId = null)
            {
                var message = new MimeMessage();

                message.From.Add(MailboxAddress.Parse(optionsMonitor.CurrentValue.Sender));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = subject;
                message.MessageId = messageId ?? "1";

                message.Body = new TextPart("html") { Text = htmlMessage };
                

                return message;
            }


           
        }
    }
}

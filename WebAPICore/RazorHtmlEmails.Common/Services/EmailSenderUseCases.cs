using Microsoft.AspNetCore.Identity.UI.Services;
using RazorHtmlEmails.RazorClassLib.Models;
using RazorHtmlEmails.RazorClassLib.Services;
using RazorHtmlEmails.RazorClassLib.Views.Emails.NewAccount;
using RazorHtmlEmails.RazorClassLib.Views.Emails.ProductRequestEmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class EmailSenderUseCases : IEmailSenderUseCases
    {
        private readonly IEmailSender emailSender;
        private readonly IRazorViewToStringRenderer razorViewToStringRenderer;

        public EmailSenderUseCases(IEmailSender emailSender, IRazorViewToStringRenderer razorViewToStringRenderer)
        {
            this.emailSender = emailSender;
            this.razorViewToStringRenderer = razorViewToStringRenderer;
        }
        public async Task CreateEmailSpecializedAsync(string email, string subject, NewAccountEmailViewModel model)
        {
            string htmlMessage = await razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/NewAccount/NewAccountEmail.cshtml", model);
            
            await emailSender.SendEmailAsync(email, subject, htmlMessage);
        }

        public async Task CreateEmailSpecializedAsync(string email, string subject, ProductRequestViewModel model)
        {
            string htmlMessage = await razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ProductRequest/ProductRequestEmail.cshtml", model);

            await emailSender.SendEmailAsync(email, subject, htmlMessage);
        }
    }
}

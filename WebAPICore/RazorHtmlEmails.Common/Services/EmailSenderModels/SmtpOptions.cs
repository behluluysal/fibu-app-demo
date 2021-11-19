using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorHtmlEmails.Common.Models.EmailSender
{
    public class SmtpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public SecureSocketOptions Security { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
        public int MaxSenderCount { get; set; }
        public TimeSpan DelayOnError { get; set; }
    }
}

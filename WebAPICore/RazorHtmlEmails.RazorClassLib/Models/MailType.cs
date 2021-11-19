using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorHtmlEmails.RazorClassLib.Models
{
    public class MailType
    {
        public MailTypeEnums MailTypes { get; set; }

        public enum MailTypeEnums
        {
            NewAccount,
            ProductRequests,
            UpdateAccount

        }
    }
}

using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorHtmlEmails.RazorClassLib.Views.Emails.NewAccount
{
    public class NewAccountEmailViewModel
    {
        public NewAccountEmailViewModel(string WebsiteUrl, CreateUserViewModel user)
        {
            this.WebsiteUrl = WebsiteUrl;
            ApplicationUser = user;
        }

        public string WebsiteUrl { get; set; }
        public CreateUserViewModel ApplicationUser { get; }
    }
}
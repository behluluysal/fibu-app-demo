using RazorHtmlEmails.RazorClassLib.Models;
using RazorHtmlEmails.RazorClassLib.Views.Emails.NewAccount;
using RazorHtmlEmails.RazorClassLib.Views.Emails.ProductRequestEmail;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public interface IEmailSenderUseCases
    {
        Task CreateEmailSpecializedAsync(string email, string subject, NewAccountEmailViewModel model);
        Task CreateEmailSpecializedAsync(string email, string subject, ProductRequestViewModel model);
    }
}
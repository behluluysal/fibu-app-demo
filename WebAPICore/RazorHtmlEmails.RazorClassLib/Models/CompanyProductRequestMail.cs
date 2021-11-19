using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorHtmlEmails.RazorClassLib.Models
{
    public class CompanyProductRequestMail : MailType
    {
        public List<RequestedProduct> RequestedProducts { get; }
        public CompanyProductRequestMail(List<RequestedProduct> requestedProducts)
        {
            RequestedProducts = requestedProducts;
            MailTypes = MailTypeEnums.ProductRequests;
        }

        
    }
}

using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorHtmlEmails.RazorClassLib.Views.Emails.ProductRequestEmail
{
    public class ProductRequestViewModel
    {
        public ProductRequestViewModel(string RequestPageUrl, ICollection<RequestedProduct> RequestedProducts)
        {
            this.RequestPageUrl = RequestPageUrl;
            this.RequestedProducts = RequestedProducts;
        }

        public string RequestPageUrl { get; }
        public ICollection<RequestedProduct> RequestedProducts { get; }
    }
}

using AutoMapper;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class OfferDto
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public PaymentDto Payment { get; set; }
        public RequestedProductDto RequestedProduct { get; set; }
    }
}

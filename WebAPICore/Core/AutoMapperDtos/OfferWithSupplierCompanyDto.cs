using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class OfferWithSupplierCompanyDto
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public int RequestedProductId { get; set; }
        public RequestedProductDto RequestedProduct { get; set; }
        public int SupplierCompanyId { get; set; }
        public SupplierCompanyWithoutContactDto SupplierCompany { get; set; }
        public int PaymentId { get; set; }
        public PaymentDto Payment { get; set; }
        public bool isConfirmedOffer { get; set; }
    }
}
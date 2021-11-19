using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class OfferCreateDto
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public int SupplierCompanyId { get; set; }
        public int RequestedProductId { get; set; }
    }
}

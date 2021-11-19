using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class BusinessPartnerWithRequestWithRequestedProductsDto : BusinessPartnerDto
    {
        public RequestWithRequestedProductDto Request { get; set; }
    }
}

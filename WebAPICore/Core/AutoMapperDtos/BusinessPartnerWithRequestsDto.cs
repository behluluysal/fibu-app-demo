using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class BusinessPartnerWithRequestsDto : BusinessPartnerDto
    {
        public ICollection<RequestDto> Requests { get; set; }
    }
}

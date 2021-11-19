using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class RequestWithRequestedProductDto : RequestDto
    {
        public virtual ICollection<RequestedProductDto> RequestedProducts { get; set; }
    }
}

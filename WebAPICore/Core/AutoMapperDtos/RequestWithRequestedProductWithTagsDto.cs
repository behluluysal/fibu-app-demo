using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class RequestWithRequestedProductWithTagsDto : RequestDto
    {
        public virtual ICollection<RequestedProductWithTagsDto> RequestedProducts { get; set; }
    }
}

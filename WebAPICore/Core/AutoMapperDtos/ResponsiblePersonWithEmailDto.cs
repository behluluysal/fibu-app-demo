using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class ResponsiblePersonWithEmailDto : ResponsiblePersonDto
    {
        public IList<SCRPEmailDto> Emails { get; set; }
    }
}

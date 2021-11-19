using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class BPResponsiblePersonWithEmailDto : BPResponsiblePersonDto
    {
        public ICollection<BPRPEmailDto> Emails { get; set; }
    }
}

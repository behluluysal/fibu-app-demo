using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class BPResponsiblePersonWithPhoneNumberDto : BPResponsiblePersonDto
    {
        public ICollection<BPRPPhoneNumberDto> PhoneNumbers { get; set; }
    }
}

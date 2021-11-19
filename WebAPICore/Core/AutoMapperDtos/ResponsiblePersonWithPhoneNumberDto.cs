using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class ResponsiblePersonWithPhoneNumberDto : ResponsiblePersonDto
    {
        public IList<SCRPPhoneNumberDto> PhoneNumbers { get; set; }
    }
}

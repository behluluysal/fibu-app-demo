using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class ResponsiblePersonWithContactDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int SupplierCompanyId { get; set; }
        public IList<SCRPEmailDto> Emails { get; set; }
        public IList<SCRPPhoneNumberDto> PhoneNumbers { get; set; }
    }
}
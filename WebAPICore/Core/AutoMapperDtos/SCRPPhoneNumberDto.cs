using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class SCRPPhoneNumberDto
    {
        public int Id { get; set; }
        public string Gsm { get; set; }
        public int ResponsiblePersonId { get; set; }
    }
}

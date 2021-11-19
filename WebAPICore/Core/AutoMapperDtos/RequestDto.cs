using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class RequestDto
    {
        public int Id { get; set; }
        public Request.StatusValues Status { get; set; }
        public int BusinessPartnerId { get; set; }
        public BusinessPartnerDto BusinessPartner { get; set; }
        public string No { get; set; }
    }
}

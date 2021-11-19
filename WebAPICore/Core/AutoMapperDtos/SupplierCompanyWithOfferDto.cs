using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class SupplierCompanyWithOfferDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gsm { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public ICollection<OfferDto> Offers { get; set; }
        public ICollection<TagDto> Tags { get; set; }

    }
}

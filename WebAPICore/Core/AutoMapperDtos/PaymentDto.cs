using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Method { get; set; }
        public DateTime Date { get; set; }
        public int OfferId { get; set; }
    }
}

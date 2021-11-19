using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class RequestedProductWithTagsDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime Deadline { get; set; }
        public int ProductId { get; set; }
        public ProductWithTagDto Product { get; set; }
        public RequestedProduct.StatusValues Status { get; set; }
    }
}

using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class RequestedProductDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime Deadline { get; set; }
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int RequestId { get; set; }
        public RequestDto Request { get; set; }
        public RequestedProduct.StatusValues Status { get; set; }
    }
}

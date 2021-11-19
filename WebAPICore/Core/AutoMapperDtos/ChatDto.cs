using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class ChatDto
    {
        public string Id { get; set; }
        public bool IsNewMessage { get; set; }
        public DateTime CreatedAt { get; set; }


        public int SupplierCompanyId { get; set; }
        public SupplierCompanyWithOfferDto SupplierCompany { get; set; }

        public int RequestId { get; set; }
        public RequestWithRequestedProductWithTagsDto Request { get; set; }

        public ICollection<MessageDto> Messages { get; set; }
    }
}

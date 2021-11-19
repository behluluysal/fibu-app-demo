using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Chats
{
    public class Chat
    {
        [Key]
        public string Id { get; set; }
        public bool IsNewMessage { get; set; }
        public DateTime CreatedAt { get; set; }


        public int SupplierCompanyId { get; set; }
        public virtual SupplierCompany SupplierCompany { get; set; }

        public int RequestId { get; set; }
        public virtual Request Request { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}

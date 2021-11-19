using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class RequestedProduct
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Quantity { get; set; }
        public DateTime Deadline { get; set; }

        public StatusValues Status { get; set; }
        

        public int RequestId { get; set; }
        public virtual Request Request { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }

        public enum StatusValues
        {
            Created,
            Approved,
            PreConfirm,
            Confirmed
        }
    }
}

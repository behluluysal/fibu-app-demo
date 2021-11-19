using Core.Models.Chats;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Request
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MinLength(15)]
        public string Token { get; set; }

        public string No { get; set; }
        public StatusValues Status { get; set; }

        public int BusinessPartnerId { get; set; }
        public virtual BusinessPartner BusinessPartner { get; set; }
        public virtual ICollection<RequestedProduct> RequestedProducts { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }


        public enum StatusValues
        {
            Created,
            Approved,
            Completed
        }
    }
}

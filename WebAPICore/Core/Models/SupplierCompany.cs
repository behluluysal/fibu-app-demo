using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Chats;

namespace Core.Models
{
    public class SupplierCompany
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gsm { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        [Required, MinLength(15)]
        public string Token { get; set; }
        public virtual ICollection<SupplierCompanyTag> SupplierCompanyTags { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }
        public virtual ICollection<ResponsiblePerson> ResponsiblePeople { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
    }
}

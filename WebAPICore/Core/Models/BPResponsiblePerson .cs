using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class BPResponsiblePerson
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }

        public int BusinessPartnerId { get; set; }
        public virtual BusinessPartner BusinessPartner { get; set; }
        public virtual ICollection<BPRPPhoneNumber> PhoneNumbers { get; set; }
        public virtual ICollection<BPRPEmail> Emails { get; set; }
    }
}

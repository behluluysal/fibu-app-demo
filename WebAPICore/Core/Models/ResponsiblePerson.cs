using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ResponsiblePerson
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }

        public int SupplierCompanyId { get; set; }
        public virtual SupplierCompany SupplierCompany { get; set; }
        public virtual ICollection<SCRPPhoneNumber> PhoneNumbers { get; set; }
        public virtual ICollection<SCRPEmail> Emails { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class BPRPPhoneNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Gsm { get; set; }


        public int BPResponsiblePersonId { get; set; }
        public virtual BPResponsiblePerson BPResponsiblePerson { get; set; }
    }
}

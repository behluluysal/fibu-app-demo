using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Payment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Method { get; set; }
        public DateTime Date { get; set; }
        public int OfferId { get; set; }
        public virtual Offer Offer { get; set; }
    }
}

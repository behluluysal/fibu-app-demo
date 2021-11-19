using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Offer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Amount { get; set; }
        public int? PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
        public int SupplierCompanyId { get; set; }
        public virtual SupplierCompany SupplierCompany { get; set; }
        public int RequestedProductId { get; set; }
        public virtual RequestedProduct RequestedProduct { get; set; }

        public bool isConfirmedOffer { get; set; }
    }
}

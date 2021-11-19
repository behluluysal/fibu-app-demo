using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class SupplierCompanyTag
    {
        public int SupplierCompanyId { get; set; }
        public virtual SupplierCompany SupplierCompany { get; set; }


        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}

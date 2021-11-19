using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class BPRPEmailDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool CanLogin { get; set; }
        public int ResponsiblePersonId { get; set; }
    }
}

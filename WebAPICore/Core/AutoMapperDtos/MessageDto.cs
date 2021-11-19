using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AutoMapperDtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public bool AdminMessage { get; set; }
        public string MessageText { get; set; }
        public DateTime SentTime { get; set; }

    }
}

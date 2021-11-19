using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Chats
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public bool AdminMessage { get; set; }
        public string MessageText { get; set; }
        public DateTime SentTime { get; set; }

        public string ChatId { get; set; }
        public virtual Chat Chat { get; set; }
    }
}

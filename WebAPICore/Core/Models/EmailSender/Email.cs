using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.EmailSender
{
    public class Email
    {
        [Key]
        public int Id { get; set; }
        public string EmailTo { get; set; }
        public string Subject{ get; set; }
        public string Message { get; set; }

        public int Tries { get; set; }
        public MailStatus Status { get; set; }
        public enum MailStatus
        {
            InProgress,
            Sent,
            Deleted
        }
    }
}

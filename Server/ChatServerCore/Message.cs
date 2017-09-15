using System.ComponentModel.DataAnnotations;

namespace ChatServerCore
{
    public class Message
    {
        public int ID { get; set; }
        [Required]
        public virtual Contact Sender { get; set; }
        [Required]
        public virtual Contact Receiver { get; set; }
        public string Date { get; set; }
        [MinLength(1), MaxLength(128)]
        public string Body { get; set; }
    }
}

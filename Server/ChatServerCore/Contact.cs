using System.ComponentModel.DataAnnotations;

namespace ChatServerCore
{
    public class Contact
    {
        public int ID { get; set; }
        [MinLength(1), MaxLength(9)]
        public string Username { get; set; }
        [MinLength(1), MaxLength(9)]
        public string Password { get; set; }
        [MinLength(1), MaxLength(20)]
        public string Name { get; set; }
        public string CreatedOn { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RWSS.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [ForeignKey("Sender")]
        public string? SenderId { get; set; }
        public AppUser? Sender { get; set; }

        [ForeignKey("Recipient")]
        public string? RecipientId { get; set; }
        public AppUser? Recipient { get; set; }

    }
}

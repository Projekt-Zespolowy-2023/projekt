using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RWSS.Models
{
    public class Assignment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Body { get; set; }

        [ForeignKey("Assignee")]
        public string? AssigneeId { get; set; }
        public AppUser? Assignee { get; set; }

        [ForeignKey("Assignor")]
        public string? AssignorId { get; set; }
        public AppUser? Assignor { get; set; }
    }
}

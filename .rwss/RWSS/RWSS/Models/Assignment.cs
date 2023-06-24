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

        [DataType(DataType.Date)]
        public DateTime DateAssigned { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfAssignment { get; set; }
        public bool IsCompleted { get; set; }

        [ForeignKey("Assignee")]
        public string? AssigneeId { get; set; } //komu przydzielony
        public AppUser? Assignee { get; set; }

        [ForeignKey("Assignor")]
        public string? AssignorId { get; set; } //kto przydzielał
        public AppUser? Assignor { get; set; }
    }
}

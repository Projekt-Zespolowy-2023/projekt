using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RWSS.Models
{
    public class Duty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Body { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateAssigned { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly TimeOfDuty { get; set; }

        [ForeignKey("Assignee")]
        public string? AssigneeId { get; set; }
        public AppUser? Assignee { get; set; }

        [ForeignKey("Assignor")]
        public string? AssignorId { get; set; }
        public AppUser? Assignor { get; set; }
    }
}

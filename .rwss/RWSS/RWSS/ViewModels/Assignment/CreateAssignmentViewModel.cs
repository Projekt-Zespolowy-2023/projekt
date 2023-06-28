using RWSS.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RWSS.ViewModels.Assignment
{
    public class CreateAssignmentViewModel
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime DateAssigned { get; set; }
        public DateTime DateOfAssignment { get; set; }
        public string? AssigneeId { get; set; }
        public string? AssignorId { get; set; }
    }
}

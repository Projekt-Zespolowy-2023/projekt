using System.ComponentModel.DataAnnotations;

namespace RWSS.ViewModels.Duties
{
    public class CreateDutyViewModel
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime DateAssigned { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly TimeOfDuty { get; set; }
        public TimeOnly EndingTime { get; set; }
        public string? AssigneeId { get; set; }
        public string? AssignorId { get; set; }
    }
}

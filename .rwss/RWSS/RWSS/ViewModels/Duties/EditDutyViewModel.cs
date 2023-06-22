using System.ComponentModel.DataAnnotations;

namespace RWSS.ViewModels.Duties
{
    public class EditDutyViewModel
    {
        public int Id { get; set; }
        public string Body { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateAssigned { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdateDate { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly TimeOfDuty { get; set; }
    }
}

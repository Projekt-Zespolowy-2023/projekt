using RWSS.Data.Enum;
using RWSS.Models;

namespace RWSS.ViewModels.Dashboard
{
    public class EditUserViewModel
    {
        public string AppUserId { get; set; }
        public int? StudentId { get; set; }
        public int? DeaneryWorkerId { get; set; }
        public string? EmailAdress { get; set; }
        public string? PhoneNumber { get; set; }
        public PositionCategory? PositionCategory { get; set; }
		public YearCategory? YearCategory { get; set; }
		public SemesterCategory? SemesterCategory { get; set; }
		public DegreeCourseCategory? DegreeCourseCategory { get; set; }
		public StudiesDegreeCategory? StudiesDegreeCategory { get; set; }
	}
}

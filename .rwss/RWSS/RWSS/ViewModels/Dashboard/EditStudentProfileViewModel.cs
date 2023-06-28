using RWSS.Data.Enum;

namespace RWSS.ViewModels.Dashboard
{
    public class EditStudentProfileViewModel
    {
        public int Id { get; set; }
        public YearCategory YearCategory { get; set; }
        public SemesterCategory SemesterCategory { get; set; }
        public DegreeCourseCategory DegreeCourseCategory { get; set; }
        public StudiesDegreeCategory StudiesDegreeCategory { get; set; }
    }
}

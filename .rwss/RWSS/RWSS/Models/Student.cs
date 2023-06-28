using RWSS.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RWSS.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public string IndexNumber { get; set; }
        public YearCategory YearCategory { get; set; }
        public SemesterCategory SemesterCategory { get; set; }
        public DegreeCourseCategory DegreeCourseCategory { get; set; } //kierunek
        public StudiesDegreeCategory StudiesDegreeCategory { get; set; } //stopień
    }
}

using RWSS.Models;

namespace RWSS.ViewModels
{
    public class StudentsByYearViewModel
    {
        public IEnumerable<Student> Students { get; set; } = null;
    }
}

using RWSS.Models;

namespace RWSS.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public List<Event> UserEvents { get; set; }
        public List<Event> Events { get; set; }
        public List<Student> Students { get; set; }
        public List<Student> firstYearStudents { get; set; }
        public List<Student> secondYearStudents { get; set; }
        public List<Student> thirdYearStudents { get; set; }
        public List<Student> RWSSStudents { get; set; }
        public List<Student> RWSSAdmin { get; set; }
        public List<DeaneryWorker> DeaneryWorkers { get; set; }
        public List<DeaneryWorker> DeaneryWorkersByPosition { get; set; }

    }
}

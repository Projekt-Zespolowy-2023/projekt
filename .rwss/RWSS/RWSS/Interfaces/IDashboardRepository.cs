using RWSS.Data.Enum;
using RWSS.Models;

namespace RWSS.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Event>> GetAllUserEvents();
        Task<List<Event>> GetAllEvents();
        Task<List<Student>> GetAllStudents();
        Task<List<Student>> GetStudentsByYear(YearCategory year);
        Task<List<Student>> GetRWSSStudents();
        Task<List<Student>> GetRWSSAdmin();
        Task<AppUser> GetByIdAsync(string id);
        Task<AppUser> GetByIdAsyncNoTracking(string id);
        Task<List<DeaneryWorker>> GetAllDeaneryWorkers();
        Task<List<DeaneryWorker>> GetAllDeaneryWorkersByPosition(PositionCategory position);
        Task<DeaneryWorker> GetDWByIdAsync(int id);
        Task<DeaneryWorker> GetDWByIdAsyncNoTracking(int id);
        bool Update(AppUser appUser);
        bool Save();
    }
}

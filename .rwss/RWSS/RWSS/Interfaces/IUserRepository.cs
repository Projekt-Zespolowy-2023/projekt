using RWSS.Models;

namespace RWSS.Interfaces
{
    public interface IUserRepository
    {
        Task<Student> GetStudentById(string id);
        Task<Student> GetStudentByIdNoTracking(string id);
        Task<DeaneryWorker> GetDeaneryWorkerById(string id);
        Task<DeaneryWorker> GetDeaneryWorkerByIdNoTracking(string id);
        Task<AppUser> GetAppUserById(string id);
        Task<AppUser> GetAppUserByIdNoTracking(string id);
        bool Update(AppUser user);
        bool UpdateStudent(Student student);
        bool UpdateDeaneryWorker(DeaneryWorker deaneryWorker);
        bool Save();
    }
}

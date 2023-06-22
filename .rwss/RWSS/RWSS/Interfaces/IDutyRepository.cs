using RWSS.Models;

namespace RWSS.Interfaces
{
    public interface IDutyRepository
    {
        Task<IEnumerable<Duty>> GetAll();
        Task<Duty> GetByIdAsync(int id);
        Task<Duty> GetByIdAsyncNoTracking(int id);
        Task<Student> GetStudentByIdAsync(int id);
        Task<Student> GetStudentByIdAsyncNoTracking(int id);
        Task<Student> GetAssignee(string id);
        Task<Student> GetAssigneeNoTracking(string id);
        Task<Student> GetAssignor(string id);
        Task<IEnumerable<Duty>> GetDutiesByStudent(string id);
        bool Add(Duty duty);
        bool Update(Duty duty);
        bool Delete(Duty duty);
        bool Save();
    }
}

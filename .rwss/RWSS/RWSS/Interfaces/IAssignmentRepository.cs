using RWSS.Models;

namespace RWSS.Interfaces
{
    public interface IAssignmentRepository
    {
        Task<IEnumerable<Assignment>> GetAll();
        Task<Assignment> GetByIdAsync(int id);
        Task<Assignment> GetByIdAsyncNoTracking(int id);
        Task<Student> GetStudentByIdAsync(int id);
        Task<Student> GetStudentByIdAsyncNoTracking(int id);
        Task<Student> GetAssignee(string id);
        Task<Student> GetAssigneeNoTracking(string id);
        Task<Student> GetAssignor(string id);
        Task<IEnumerable<Assignment>> GetAssignmentsByStudent(string id);
        bool Add(Assignment assignment);
        bool Update(Assignment assignment);
        bool Delete(Assignment assignment);
        bool Save();
    }
}

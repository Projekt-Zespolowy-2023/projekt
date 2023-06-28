using Microsoft.EntityFrameworkCore;
using RWSS.Data;
using RWSS.Interfaces;
using RWSS.Models;

namespace RWSS.Repository
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly ApplicationDbContext _context;
        public AssignmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Assignment assignment)
        {
            _context.Add(assignment);
            return Save();
        }

        public bool Delete(Assignment assignment)
        {
            _context.Remove(assignment);
            return Save();
        }

        public async Task<IEnumerable<Assignment>> GetAll()
        {
            return await _context.Assignments.Include(a => a.Assignee).ToListAsync();
        }

        public async Task<Assignment?> GetByIdAsync(int id)
        {
            return await _context.Assignments.Include(a => a.Assignee).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Assignment?> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Assignments.Include(a => a.Assignee).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Assignment>> GetAssignmentsByStudent(string id)
        {
            return await _context.Assignments.Include(a => a.Assignor).Where(c => c.AssigneeId == id).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Assignment assignment)
        {
            _context.Update(assignment);
            return Save();
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _context.Students.Include(a => a.AppUser).FirstOrDefaultAsync(i  => i.Id == id);
        }

        public async Task<Student> GetStudentByIdAsyncNoTracking(int id)
        {
            return await _context.Students.AsNoTracking().Include(a => a.AppUser).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Student> GetAssignee(string id)
        {
            return await _context.Students.Include(a => a.AppUser).FirstOrDefaultAsync(a => a.AppUser.Id == id);
        }

        public async Task<Student> GetAssigneeNoTracking(string id)
        {
            return await _context.Students.AsNoTracking().Include(a => a.AppUser).FirstOrDefaultAsync(a => a.AppUser.Id == id);
        }

        public async Task<Student> GetAssignor(string id)
        {
            return await _context.Students.Include(a => a.AppUser).FirstOrDefaultAsync(a => a.AppUser.Id == id);
        }
    }
}

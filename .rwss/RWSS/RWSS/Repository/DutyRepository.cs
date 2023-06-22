using Microsoft.EntityFrameworkCore;
using RWSS.Data;
using RWSS.Interfaces;
using RWSS.Models;

namespace RWSS.Repository
{
    public class DutyRepository : IDutyRepository
    {
        private readonly ApplicationDbContext _context;
        public DutyRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Duty duty)
        {
            _context.Add(duty);
            return Save();
        }

        public bool Delete(Duty duty)
        {
            _context.Remove(duty);
            return Save();
        }

        public async Task<IEnumerable<Duty>> GetAll()
        {
            return await _context.Duties.Include(a => a.Assignee).ToListAsync();
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

        public async Task<Duty> GetByIdAsync(int id)
        {
            return await _context.Duties.Include(a => a.Assignee).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Duty> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Duties.Include(a => a.Assignee).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Duty>> GetDutiesByStudent(string id)
        {
            return await _context.Duties.Include(a => a.Assignor).Where(c => c.AssigneeId == id).ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _context.Students.AsNoTracking().Include(a => a.AppUser).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Student> GetStudentByIdAsyncNoTracking(int id)
        {
            return await _context.Students.AsNoTracking().Include(a => a.AppUser).FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Duty duty)
        {
            _context.Update(duty);
            return Save();
        }
    }
}

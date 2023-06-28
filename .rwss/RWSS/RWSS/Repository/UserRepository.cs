using Microsoft.EntityFrameworkCore;
using RWSS.Data;
using RWSS.Interfaces;
using RWSS.Models;

namespace RWSS.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Student> GetStudentById(string id)
        {
            return await _context.Students.Include(a => a.AppUser).FirstOrDefaultAsync(i => i.AppUser.Id == id);
        }

        public async Task<Student> GetStudentByIdNoTracking(string id)
        {
            return await _context.Students.Include(a => a.AppUser).AsNoTracking().FirstOrDefaultAsync(i => i.AppUser.Id == id);
        }

        public async Task<DeaneryWorker> GetDeaneryWorkerById(string id)
        {
            return await _context.DeaneryWorkers.Include(a => a.AppUser).FirstOrDefaultAsync(i => i.AppUser.Id == id);
        }

        public async Task<DeaneryWorker> GetDeaneryWorkerByIdNoTracking(string id)
        {
            return await _context.DeaneryWorkers.Include(a => a.AppUser).AsNoTracking().FirstOrDefaultAsync(i => i.AppUser.Id == id);
        }

        public async Task<AppUser> GetAppUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetAppUserByIdNoTracking(string id)
        {
            return await _context.Users.Where(a => a.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            _context.Update(user);
            return Save();
        }

		public bool UpdateStudent(Student student)
		{
			_context.Update(student);
			return Save();
		}

		public bool UpdateDeaneryWorker(DeaneryWorker deaneryWorker)
		{
			_context.Update(deaneryWorker);
			return Save();
		}
	}
}

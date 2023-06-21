using Microsoft.EntityFrameworkCore;
using RWSS.Data;
using RWSS.Data.Enum;
using RWSS.Interfaces;
using RWSS.Models;

namespace RWSS.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Student>> GetAllStudents()
        {
            var students = _context.Students.Include(a => a.AppUser);

            return students.ToList();
        }

        public async Task<List<Student>> GetStudentsByYear(YearCategory year)
        {
            var studentsByYear = _context.Students.Include(a => a.AppUser).Where(r => r.YearCategory.Equals(year));

            return studentsByYear.ToList();
        }

        public async Task<List<Student>> GetRWSSStudents()
        {
            var rwssStudents = _context.Students.Include(a => a.AppUser).Where(r => r.AppUser.RoleCategory.Equals(RoleCategory.Członek_RWSS));

            return rwssStudents.ToList();
        }

        public async Task<List<Student>> GetRWSSAdmin()
        {
            var rwssAdmin = _context.Students.Include(a => a.AppUser).Where(r => r.AppUser.RoleCategory.Equals(RoleCategory.Admin_RWSS));

            return rwssAdmin.ToList();
        }

        public async Task<AppUser> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetByIdAsyncNoTracking(string id)
        {
            return _context.Users.AsNoTracking().FirstOrDefault(i => i.Id == id);
        }

        public async Task<List<Event>> GetAllUserEvents()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var curStudent = _context.Students.FirstOrDefault(r => r.AppUserId == curUser);
            var events = _context.Events.Where(r => r.YearCategory.Equals(curStudent.YearCategory));

            return events.ToList();
        }

        public async Task<List<Event>> GetAllEvents()
        {
            var events = _context.Events;

            return events.ToList();
        }

        public async Task<List<DeaneryWorker>> GetAllDeaneryWorkers()
        {
            var deaneryWorkers = _context.DeaneryWorkers.Include(a => a.AppUser);

            return deaneryWorkers.ToList();
        }

        public async Task<List<DeaneryWorker>> GetAllDeaneryWorkersByPosition(PositionCategory position)
        {
            var deaneryWorkers = _context.DeaneryWorkers.Include(a => a.AppUser).Where(r => r.PositionCategory.Equals(position));

            return deaneryWorkers.ToList();
        }

        public async Task<DeaneryWorker> GetDWByIdAsync(int id)
        {
            var deaneryWorker = _context.DeaneryWorkers.FirstOrDefault(i => i.Id == id);

            return deaneryWorker;
        }
        public async Task<DeaneryWorker> GetDWByIdAsyncNoTracking(int id)
        {
            var deaneryWorker = _context.DeaneryWorkers.AsNoTracking().FirstOrDefault(i => i.Id == id);

            return deaneryWorker;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser appUser)
        {
            _context.Update(appUser);
            return Save();
        }
    }
}

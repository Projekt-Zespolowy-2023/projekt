using Microsoft.EntityFrameworkCore;
using RWSS.Data;
using RWSS.Data.Enum;
using RWSS.Interfaces;
using RWSS.Models;
using RWSS.ViewModels.Events;

namespace RWSS.Repository
{
    public class EventRepository : IEventRepository
	{
		private readonly ApplicationDbContext _context;
		public EventRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public bool Add(Event eve)
		{
			_context.Add(eve);
			return Save();
		}

		public bool Delete(Event eve)
		{
			_context.Remove(eve);
			return Save();
		}

        public async Task<IEnumerable<Student>> GetStudentsByYear(YearCategory year)
		{
			return await _context.Students.Include(a => a.AppUser).Where(y => y.YearCategory.Equals(year)).ToListAsync();
		}

        public async Task<IEnumerable<Event>> GetAll()
		{
			return await _context.Events.ToListAsync();
		}

		public async Task<Event> GetByIdAsync(int id)
		{
			return await _context.Events.FirstOrDefaultAsync(i => i.Id == id);
		}

		public async Task<Event> GetByIdAsyncNoTracking(int id)
		{
			return await _context.Events.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
		}

		public async Task<IEnumerable<Event>> GetEventsByCategory(string category)
		{
			return await _context.Events.Where(c => c.EventCategory.Equals(category)).ToListAsync();
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool Update(Event eve)
		{
			_context.Update(eve);
			return Save();
		}
	}
}

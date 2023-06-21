using RWSS.Data.Enum;
using RWSS.Models;
using RWSS.ViewModels.Events;

namespace RWSS.Interfaces
{
    public interface IEventRepository
	{
		Task<IEnumerable<Event>> GetAll();
		Task<Event> GetByIdAsync(int id);
		Task<Event> GetByIdAsyncNoTracking(int id);
		Task<IEnumerable<Event>> GetEventsByCategory(string category);
		Task<IEnumerable<Student>> GetStudentsByYear(YearCategory year);
		bool Add(Event eve);
		bool Update(Event eve);
		bool Delete(Event eve);
		bool Save();
	}
}

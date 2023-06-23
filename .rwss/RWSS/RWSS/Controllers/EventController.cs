using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RWSS.Data;
using RWSS.Interfaces;
using RWSS.Models;
using RWSS.ViewModels.Events;

namespace RWSS.Controllers
{
    public class EventController : Controller
	{
		private readonly IEventRepository _eventRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public EventController(IEventRepository eventRepository, IHttpContextAccessor httpContextAccessor)
		{
			_eventRepository = eventRepository;
			_httpContextAccessor = httpContextAccessor;
		}
		public async Task<IActionResult> Index()
		{
			IEnumerable<Event> events = await _eventRepository.GetAll();
			return View(events);
		}

		public async Task<IActionResult> Detail(int id)
		{
			Event eve = await _eventRepository.GetByIdAsync(id);
			return View(eve);
		}

		[HttpGet]
        public IActionResult Create()
		{
			var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
			var createEventVM = new CreateEventViewModel { AppUserId = curUserId };
			return View(createEventVM);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateEventViewModel createEventVM)
		{
			if (!ModelState.IsValid)
			{
				return View(createEventVM);
			}

			var eve = new Event
			{
				Title = createEventVM.Title,
				Description = createEventVM.Description,
				PlaceOfEventCategory = createEventVM.PlaceOfEventCategory,
				CreationDate = DateTime.Now,
				UpdateDate = DateTime.Now,
				EventDate = createEventVM.EventDate,
				EventCategory = createEventVM.EventCategory,
				AppUserId = createEventVM.AppUserId,
				YearCategory = createEventVM.YearCategory,
			};

			_eventRepository.Add(eve);
			return RedirectToAction("Index", "Dashboard");
		}

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
		{
			var eve = await _eventRepository.GetByIdAsync(id);
			if(eve == null)
			{
				return View("Error");
			}
			var eveVM = new EditEventViewModel
			{
				Title = eve.Title,
				Description = eve.Description,
				EventCategory = eve.EventCategory,
			};
			return View(eveVM);
		}

        [HttpPost]
		public async Task<IActionResult> Edit(int id, EditEventViewModel eveVM)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Failed to edit the event");
				return View("Edit", eveVM);
			}

			var userEvent = await _eventRepository.GetByIdAsyncNoTracking(id);

			var eve = new Event
			{
				Id = id,
				Title = eveVM.Title,
				Description = eveVM.Description,
				EventCategory = eveVM.EventCategory,
                CreationDate = userEvent.CreationDate,
                UpdateDate = DateTime.Now,
                EventDate = eveVM.EventDate,
				YearCategory = eveVM.YearCategory
            };
			_eventRepository.Update(eve);

			return RedirectToAction("Index", "Dashboard");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var eventDetails = await _eventRepository.GetByIdAsync(id);
			if (eventDetails == null)
			{
				return View("Error");
			}
			return View(eventDetails);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteEvent(int id)
		{
			var eventDetails = await _eventRepository.GetByIdAsync(id);
            if (eventDetails == null)
            {
                return View("Error");
            }
			_eventRepository.Delete(eventDetails);
			return RedirectToAction("Index", "Dashboard");
        }
	}
}

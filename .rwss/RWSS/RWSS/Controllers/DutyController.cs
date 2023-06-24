using Microsoft.AspNetCore.Mvc;
using RWSS.Interfaces;
using RWSS.Models;
using RWSS.Repository;
using RWSS.ViewModels.Duties;

namespace RWSS.Controllers
{
    public class DutyController : Controller
    {
        private readonly IDutyRepository _dutyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DutyController(IDutyRepository dutyRepository, IHttpContextAccessor httpContextAccessor)
        {
            _dutyRepository = dutyRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Duty> duties = await _dutyRepository.GetAll();
            return View(duties);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Duty duty = await _dutyRepository.GetByIdAsync(id);
            return View(duty);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var rwssUser = await _dutyRepository.GetStudentByIdAsync(id);
            var createDutyVM = new CreateDutyViewModel
            {
                AssignorId = curUserId,
                AssigneeId = rwssUser.AppUser.Id,
            };
            return View(createDutyVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDutyViewModel createDutyVM, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(createDutyVM);
            }

            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var rwssUser = await _dutyRepository.GetAssignee(createDutyVM.AssigneeId);
            var assignor = await _dutyRepository.GetAssignor(curUserId);
            var duty = new Duty
            {
                Body = createDutyVM.Body,
                DateAssigned = DateTime.Now,
                UpdateDate = DateTime.Now,
                DayOfWeek = createDutyVM.DayOfWeek,
                TimeOfDuty = createDutyVM.TimeOfDuty,
                IsStarted = false,
                IsCompleted = true,
                EndingTime = createDutyVM.EndingTime,
                Assignee = rwssUser.AppUser,
                Assignor = assignor.AppUser,
            };
            _dutyRepository.Add(duty);
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> GetMyDuties()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var myDuties = await _dutyRepository.GetDutiesByStudent(curUserId);
            return View(myDuties);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var duty = await _dutyRepository.GetByIdAsync(id);
            if (duty == null)
            {
                return View("Error");
            }
            var dutyVM = new EditDutyViewModel
            {
                Body = duty.Body,
                UpdateDate = duty.UpdateDate,
                DateAssigned = duty.DateAssigned,
                DayOfWeek = duty.DayOfWeek,
                TimeOfDuty = duty.TimeOfDuty,
                EndingTime = duty.EndingTime,
            };
            return View(dutyVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditDutyViewModel dutyVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit the assignment");
                return View("Edit", dutyVM);
            }

            var userDuty = await _dutyRepository.GetByIdAsyncNoTracking(id);

            if (userDuty == null)
            {
                return View("Error");
            }

            var duty = new Duty
            {
                Id = id,
                Body = dutyVM.Body,
                Assignee = userDuty.Assignee,
                AssigneeId = userDuty.AssigneeId,
                Assignor = userDuty.Assignor,
                AssignorId = userDuty.AssignorId,
                DateAssigned = dutyVM.DateAssigned,
                UpdateDate = DateTime.Now,
                DayOfWeek = dutyVM.DayOfWeek,
                TimeOfDuty = dutyVM.TimeOfDuty,
                EndingTime = dutyVM.EndingTime,
            };
            _dutyRepository.Update(duty);
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dutyDetails = await _dutyRepository.GetByIdAsync(id);
            if (dutyDetails == null)
            {
                return View("Error");
            }
            return View(dutyDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteDuty(int id)
        {
            var dutyDetails = await _dutyRepository.GetByIdAsync(id);
            if (dutyDetails == null)
            {
                return View("Error");
            }
            _dutyRepository.Delete(dutyDetails);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Register(int id)
        {
            var duty = await _dutyRepository.GetByIdAsync(id);
            if (duty == null)
            {
                return View("Error");
            }
            return View(duty);
        }

        [HttpPost, ActionName("Register")]
        public async Task<IActionResult> RegisterDuty(int id)
        {
            var duty = await _dutyRepository.GetByIdAsyncNoTracking(id);
            if (duty == null)
            {
                return View("Error");
            }
            duty.IsStarted = true;
            duty.IsCompleted = false;
            _dutyRepository.Update(duty);
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Unregister(int id)
        {
            var duty = await _dutyRepository.GetByIdAsync(id);
            if (duty == null)
            {
                return View("Error");
            }
            return View(duty);
        }

        [HttpPost, ActionName("Unregister")]
        public async Task<IActionResult> UnregisterDuty(int id)
        {
            var duty = await _dutyRepository.GetByIdAsyncNoTracking(id);
            if (duty == null)
            {
                return View("Error");
            }
            duty.IsStarted = false;
            duty.IsCompleted = true;
            _dutyRepository.Update(duty);
            return RedirectToAction("Index","Dashboard");
        }
    }
}

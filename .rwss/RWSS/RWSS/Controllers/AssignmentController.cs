using Microsoft.AspNetCore.Mvc;
using RWSS.Interfaces;
using RWSS.Models;
using RWSS.ViewModels.Assignment;

namespace RWSS.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AssignmentController(IAssignmentRepository assignmentRepository, IHttpContextAccessor httpContextAccessor)
        {
            _assignmentRepository = assignmentRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Assignment> assignments = await _assignmentRepository.GetAll();
            return View(assignments);
        } //możliwe że niepotrzebne

        public async Task<IActionResult> Detail(int id)
        {
            Assignment assignment = await _assignmentRepository.GetByIdAsync(id);
            return View(assignment);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var rwssUser = await _assignmentRepository.GetStudentByIdAsync(id);
            var createAssignmentVM = new CreateAssignmentViewModel
            {
                AssignorId = curUserId,
                AssigneeId = rwssUser.AppUser.Id,
            };
            return View(createAssignmentVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAssignmentViewModel createAssignmentVM, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(createAssignmentVM);
            }

            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var rwssUser = await _assignmentRepository.GetAssignee(createAssignmentVM.AssigneeId);
            var assignor = await _assignmentRepository.GetAssignor(curUserId);
            var assignment = new Assignment
            {
                Body = createAssignmentVM.Body,
                DateAssigned = DateTime.Now,
                UpdateDate = DateTime.Now,
                DateOfAssignment = createAssignmentVM.DateOfAssignment,
                Assignee = rwssUser.AppUser,
                Assignor = assignor.AppUser,
            };

            _assignmentRepository.Add(assignment);
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> GetMyAssignments()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var myAssignments = await _assignmentRepository.GetAssignmentsByStudent(curUserId);
            return View(myAssignments);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            if (assignment == null)
            {
                return View("Error");
            }
            var assignmentVM = new EditAssignmentViewModel
            {
                Body = assignment.Body,
                UpdateDate = assignment.UpdateDate,
                DateAssigned = assignment.DateAssigned,
                DateOfAssignment = assignment.DateOfAssignment,
            };
            return View(assignmentVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditAssignmentViewModel assignmentVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit the assignment");
                return View("Edit", assignmentVM);
            }

            var userAssignment = await _assignmentRepository.GetByIdAsyncNoTracking(id);

            if (userAssignment == null)
            {
                return View("Error");
            }

            var assignment = new Assignment
            {
                Id = id,
                Body = assignmentVM.Body,
                Assignee = userAssignment.Assignee,
                AssigneeId = userAssignment.AssigneeId,
                Assignor = userAssignment.Assignor,
                AssignorId = userAssignment.AssignorId,
                DateAssigned = assignmentVM.DateAssigned,
                UpdateDate = DateTime.Now,
                DateOfAssignment = assignmentVM.DateOfAssignment,
            };
            _assignmentRepository.Update(assignment);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var assignmentDetails = await _assignmentRepository.GetByIdAsync(id);
            if (assignmentDetails == null)
            {
                return View("Error");
            }
            return View(assignmentDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var assignmentDetails = await _assignmentRepository.GetByIdAsync(id);
            if (assignmentDetails == null)
            {
                return View("Error");
            }
            _assignmentRepository.Delete(assignmentDetails);
            return RedirectToAction("Index");
        }
    }
}

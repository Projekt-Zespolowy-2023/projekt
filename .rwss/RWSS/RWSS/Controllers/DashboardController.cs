using MailKit.Net.Smtp;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using Org.BouncyCastle.Cms;
using RWSS.Data;
using RWSS.Data.Enum;
using RWSS.Interfaces;
using RWSS.Models;
using RWSS.ViewModels.Dashboard;

namespace RWSS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DashboardController(IDashboardRepository dashboardRepository, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _dashboardRepository = dashboardRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            var events = await _dashboardRepository.GetAllEvents();
            var students = await _dashboardRepository.GetAllStudents();
            var firstYearStudents = await _dashboardRepository.GetStudentsByYear(YearCategory.I);
            var secondYearStudents = await _dashboardRepository.GetStudentsByYear(YearCategory.II);
            var thirdYearStudents = await _dashboardRepository.GetStudentsByYear(YearCategory.III);
            var rwssStudents = await _dashboardRepository.GetRWSSStudents();
            var rwssAdmin = await _dashboardRepository.GetRWSSAdmin();
            var deaneryWorkers = await _dashboardRepository.GetAllDeaneryWorkers();

            var dashboardVM = new DashboardViewModel()
            {
                Events = events,
                Students = students,
                firstYearStudents = firstYearStudents,
                secondYearStudents = secondYearStudents,
                thirdYearStudents = thirdYearStudents,
                RWSSStudents = rwssStudents,
                RWSSAdmin = rwssAdmin,
                DeaneryWorkers = deaneryWorkers
            };

            if (User.IsInRole("student") || User.IsInRole("rwssuser") || User.IsInRole("rwssadmin"))
            {
                var userEvents = await _dashboardRepository.GetAllUserEvents();
                dashboardVM.UserEvents = userEvents;
            }


            return View(dashboardVM);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfileRoles(string id)
        {
            var user = await _dashboardRepository.GetByIdAsync(id);
            if (user == null)
            {
                return View("Error");
            }

            var profileRolesVM = new ProfileRolesViewModel()
            {
                RoleCategory = user.RoleCategory,
            };

            return View(profileRolesVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfileRoles(string id, ProfileRolesViewModel profileRolesVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit student roles");
                return View("EditProfileRoles", profileRolesVM);
            }

            var student = await _dashboardRepository.GetByIdAsyncNoTracking(id);
            var curUserRole = student.RoleCategory;

            var appUser = new AppUser()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                PeselNumber = student.PeselNumber,
                RoleCategory = profileRolesVM.RoleCategory,
                BirthDate = student.BirthDate,
                Email = student.Email,
                NormalizedEmail = student.NormalizedEmail,
                PasswordHash = student.PasswordHash,
                UserName = student.UserName,
                NormalizedUserName = student.NormalizedUserName,
                EmailConfirmed = student.EmailConfirmed,
                SecurityStamp = student.SecurityStamp,
                ConcurrencyStamp = student.ConcurrencyStamp,
                PhoneNumber = student.PhoneNumber,
                PhoneNumberConfirmed = student.PhoneNumberConfirmed,
                TwoFactorEnabled = student.TwoFactorEnabled,
                LockoutEnabled = student.LockoutEnabled,
                AccessFailedCount = student.AccessFailedCount,
             };

            _dashboardRepository.Update(appUser);

            if (appUser.RoleCategory == RoleCategory.Student)
            {
                var roles = await _userManager.GetRolesAsync(appUser);
                await _userManager.RemoveFromRolesAsync(appUser, roles.ToArray());
                await _userManager.AddToRoleAsync(appUser, UserRoles.Student);
            }
            else if (appUser.RoleCategory == RoleCategory.Członek_RWSS)
            {
                var roles = await _userManager.GetRolesAsync(appUser);
                await _userManager.RemoveFromRolesAsync(appUser, roles.ToArray());
                await _userManager.AddToRoleAsync(appUser, UserRoles.RWSSUser);
            }
            else if (appUser.RoleCategory == RoleCategory.Admin_RWSS)
            {
                var roles = await _userManager.GetRolesAsync(appUser);
                await _userManager.RemoveFromRolesAsync(appUser, roles.ToArray());
                await _userManager.AddToRoleAsync(appUser, UserRoles.RWSSAdmin);
            }
            else
            {
                var roles = await _userManager.GetRolesAsync(appUser);
                await _userManager.RemoveFromRolesAsync(appUser, roles.ToArray());
                await _userManager.AddToRoleAsync(appUser, UserRoles.DeaneryWorker);
            }
            

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> SelectDeaneryWorker()
        {
            var deaneryWorkers = await _dashboardRepository.GetAllDeaneryWorkers();

            return View(deaneryWorkers);
        }
    }
}

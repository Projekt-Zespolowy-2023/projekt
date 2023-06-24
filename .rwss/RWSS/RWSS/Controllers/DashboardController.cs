using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Cms;
using RWSS.Data;
using RWSS.Data.Enum;
using RWSS.Interfaces;
using RWSS.Models;
using RWSS.ViewModels.Dashboard;
using Azure.Core;
using System.Runtime.InteropServices;

namespace RWSS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        public DashboardController(IDashboardRepository dashboardRepository,
                                   UserManager<AppUser> userManager,
                                   IHttpContextAccessor httpContextAccessor,
                                   IUserRepository userRepository)
        {
            _dashboardRepository = dashboardRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
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

            var roles = await _userManager.GetRolesAsync(appUser);
            await _userManager.RemoveFromRolesAsync(appUser, roles.ToArray());

            if (appUser.RoleCategory == RoleCategory.Student)
            {
                await _userManager.AddToRoleAsync(appUser, UserRoles.Student);
            }
            else if (appUser.RoleCategory == RoleCategory.Członek_RWSS)
            {
                await _userManager.AddToRoleAsync(appUser, UserRoles.RWSSUser);
            }
            else if (appUser.RoleCategory == RoleCategory.Admin_RWSS)
            {
                await _userManager.AddToRoleAsync(appUser, UserRoles.RWSSAdmin);
            }
            else
            {
                await _userManager.AddToRoleAsync(appUser, UserRoles.DeaneryWorker);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var user = await _userRepository.GetAppUserById(curUserId);
            if (user == null)
            {
                return View("Error");
            }
            var editUserVM = new EditUserViewModel()
            {
                EmailAdress = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return View(editUserVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserViewModel editUserVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit student roles");
                return View("EditUserProfile", editUserVM);
            }

            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var user = await _userRepository.GetAppUserByIdNoTracking(curUserId);

            var appUser = new AppUser()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PeselNumber = user.PeselNumber,
                RoleCategory = user.RoleCategory,
                BirthDate = user.BirthDate,
                Email = editUserVM.EmailAdress,
                NormalizedEmail = editUserVM.EmailAdress.ToUpper(),
                PasswordHash = user.PasswordHash,
                UserName = editUserVM.EmailAdress,
                NormalizedUserName = editUserVM.EmailAdress.ToUpper(),
                EmailConfirmed = user.EmailConfirmed,
                SecurityStamp = user.SecurityStamp,
                ConcurrencyStamp = user.ConcurrencyStamp,
                PhoneNumber = editUserVM.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                LockoutEnabled = user.LockoutEnabled,
                AccessFailedCount = user.AccessFailedCount,
            };
            _userRepository.Update(appUser);

            var roles = await _userManager.GetRolesAsync(appUser);
            await _userManager.RemoveFromRolesAsync(appUser, roles.ToArray());

            if (appUser.RoleCategory == RoleCategory.Student)
            {
                await _userManager.AddToRoleAsync(appUser, UserRoles.Student);
            }
            else if (appUser.RoleCategory == RoleCategory.Członek_RWSS)
            {
                await _userManager.AddToRoleAsync(appUser, UserRoles.RWSSUser);
            }
            else if (appUser.RoleCategory == RoleCategory.Admin_RWSS)
            {
                await _userManager.AddToRoleAsync(appUser, UserRoles.RWSSAdmin);
            }
            else
            {
                await _userManager.AddToRoleAsync(appUser, UserRoles.DeaneryWorker);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> EditStudentProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var student = await _userRepository.GetStudentById(curUserId);
            if (student == null)
            {
                return View("Error");
            }

            var editStudentVM = new EditStudentProfileViewModel
            {
                YearCategory = student.YearCategory,
                SemesterCategory = student.SemesterCategory,
                DegreeCourseCategory = student.DegreeCourseCategory,
                StudiesDegreeCategory = student.StudiesDegreeCategory,
            };

            return View(editStudentVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditStudentProfile(EditStudentProfileViewModel editStudentVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit student profile");
                return View("EditStudentProfile", editStudentVM);
            }

            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var curUser = await _userRepository.GetAppUserByIdNoTracking(curUserId);
            var student = await _userRepository.GetStudentByIdNoTracking(curUserId);

            var newStudent = new Student
            {
                Id = student.Id,
                AppUserId = curUserId,
                AppUser = curUser,
                IndexNumber = student.IndexNumber,
                YearCategory = editStudentVM.YearCategory,
                SemesterCategory = editStudentVM.SemesterCategory,
                DegreeCourseCategory = editStudentVM.DegreeCourseCategory,
                StudiesDegreeCategory = editStudentVM.StudiesDegreeCategory,
            };

            _userRepository.UpdateStudent(newStudent);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> EditDeaneryWorkerProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var deaneryWorker = await _userRepository.GetDeaneryWorkerById(curUserId);
            if (deaneryWorker == null)
            {
                return View("Error");
            }

            var editDeaneryWorkerVM = new EditDeaneryWorkerProfileViewModel
            {
                PositionCategory = deaneryWorker.PositionCategory,
            };

            return View(editDeaneryWorkerVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditDeaneryWorkerProfile(EditDeaneryWorkerProfileViewModel editDeaneryWorkerVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit deanery worker profile");
                return View("EditDeaneryWorkerProfile", editDeaneryWorkerVM);
            }

            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var curUser = await _userRepository.GetAppUserByIdNoTracking(curUserId);
            var deaneryWorker = await _userRepository.GetDeaneryWorkerByIdNoTracking(curUserId);

            var newDeaneryWorker = new DeaneryWorker
            {
                Id = deaneryWorker.Id,
                AppUserId = curUserId,
                AppUser = curUser,
                PositionCategory = editDeaneryWorkerVM.PositionCategory,
            };

            _userRepository.UpdateDeaneryWorker(newDeaneryWorker);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> SelectDeaneryWorker()
        {
            var deaneryWorkers = await _dashboardRepository.GetAllDeaneryWorkers();

            return View(deaneryWorkers);
        }

        [HttpGet]
        public async Task<IActionResult> SelectRWSSUser()
        {
            var rwssUsers = await _dashboardRepository.GetAllRWSSUsers();

            return View(rwssUsers);
        }

        [HttpGet]
        public async Task<IActionResult> SelectRWSSAdmin()
        {
            var rwssAdmins = await _dashboardRepository.GetAllRWSSAdmins();

            return View(rwssAdmins);
        }
    }
}

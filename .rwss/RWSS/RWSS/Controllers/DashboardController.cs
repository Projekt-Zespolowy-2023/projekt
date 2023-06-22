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
        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var user = await _userRepository.GetAppUserById(curUserId);
            if (user == null)
            {
                return View("Error");
            }
            var editUserViewModel = new EditUserViewModel()
            {
                AppUserId = curUserId,
                EmailAdress = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserViewModel editUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editUserViewModel);
            }

            var user = await _userRepository.GetAppUserByIdNoTracking(editUserViewModel.AppUserId);
            var userRole = await _userManager.GetRolesAsync(user);
            var newUser = new AppUser
            {
                Id = user.Id,
                Email = editUserViewModel.EmailAdress,
                PhoneNumber = editUserViewModel.PhoneNumber,
                UserName = editUserViewModel.EmailAdress,
                NormalizedUserName = editUserViewModel.EmailAdress.ToUpper(),
                NormalizedEmail = editUserViewModel.EmailAdress.ToUpper(),
                EmailConfirmed = user.EmailConfirmed,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                ConcurrencyStamp = user.ConcurrencyStamp,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                LockoutEnd = user.LockoutEnd,
                LockoutEnabled = user.LockoutEnabled,
                AccessFailedCount = user.AccessFailedCount,
                BirthDate = user.BirthDate,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PeselNumber = user.PeselNumber,
                RoleCategory = user.RoleCategory,
            };

            if (user.RoleCategory == RoleCategory.Pracownik_Dziekanatu)
            {
                var deaneryWorker = await _userRepository.GetDeaneryWorkerById(user.Id);
                editUserViewModel.DeaneryWorkerId = deaneryWorker.Id;
                editUserViewModel.PositionCategory = deaneryWorker.PositionCategory;
                var newDW = new DeaneryWorker
                {
                    Id = (int)editUserViewModel.DeaneryWorkerId,
                    PositionCategory = (PositionCategory)editUserViewModel.PositionCategory,
                    AppUserId = user.Id,
                    AppUser = newUser,
                };
                _userRepository.Update(newUser);
				await _userManager.AddToRolesAsync(newUser, userRole);
				_userRepository.UpdateDeaneryWorker(newDW);
                
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                var student = await _userRepository.GetStudentById(user.Id);
                editUserViewModel.StudentId = student.Id;
				editUserViewModel.YearCategory = student.YearCategory;
				editUserViewModel.SemesterCategory = student.SemesterCategory;
				editUserViewModel.DegreeCourseCategory = student.DegreeCourseCategory;
				editUserViewModel.StudiesDegreeCategory = student.StudiesDegreeCategory;
                var newStudent = new Student
                {
                    Id = (int)editUserViewModel.StudentId,
                    YearCategory = (YearCategory)editUserViewModel.YearCategory,
                    SemesterCategory = (SemesterCategory)editUserViewModel.SemesterCategory,
                    DegreeCourseCategory = (DegreeCourseCategory)editUserViewModel.DegreeCourseCategory,
                    StudiesDegreeCategory = (StudiesDegreeCategory)editUserViewModel.StudiesDegreeCategory,
                    AppUserId = user.Id,
                };
                _userRepository.Update(newUser);
				await _userManager.AddToRolesAsync(newUser, userRole);
				_userRepository.UpdateStudent(student);
				
				return RedirectToAction("Index", "Dashboard");
			}
        }

        [HttpGet]
        public async Task<IActionResult> SelectDeaneryWorker()
        {
            var deaneryWorkers = await _dashboardRepository.GetAllDeaneryWorkers();

            return View(deaneryWorkers);
        }
    }
}

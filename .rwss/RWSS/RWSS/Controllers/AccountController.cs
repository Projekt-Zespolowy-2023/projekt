using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RWSS.Data;
using RWSS.Data.Enum;
using RWSS.Models;
using RWSS.ViewModels.Login;
using RWSS.ViewModels.Registration;

namespace RWSS.Controllers
{
    public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ApplicationDbContext _context;

		public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
		}

		[HttpGet]
		public IActionResult Login()
		{
			var response = new LoginViewModel();
			return View(response);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if(!ModelState.IsValid) 
			{
				return View(loginVM);
			}
			var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);

			if (user != null) 
			{
				var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
				if (passwordCheck)
				{
					var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
					if (result.Succeeded) 
					{
						return RedirectToAction("Index", "Dashboard");
					}
				}
				TempData["Error"] = "Złe hasło";
				return View(loginVM);
			}
			TempData["Error"] = "Zły adres email";
			return View(loginVM);
		}

        [HttpGet]
        public IActionResult Register()
		{
			var response = new RegisterViewModel();
			return View(response);
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerVM)
		{
			if (!ModelState.IsValid) 
			{
				return View(registerVM);
			}

			var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
			if (user != null) 
			{
				TempData["Error"] = "This email address is already in use";
				return View(registerVM);
			}

			
			var newUser = new AppUser()
			{
				RoleCategory = RoleCategory.Student,
				FirstName = registerVM.FirstName,
				LastName = registerVM.LastName,
				PeselNumber = registerVM.PeselNumber,
				BirthDate = registerVM.BirthDate,
				Email = registerVM.EmailAddress,
				UserName = registerVM.EmailAddress,
			};

            var newStudent = new Student()
            {
                IndexNumber = registerVM.IndexNumber,
				YearCategory = registerVM.YearCategory,
				SemesterCategory = registerVM.SemesterCategory,
				DegreeCourseCategory = registerVM.DegreeCourseCategory,
				StudiesDegreeCategory = registerVM.StudiesDegreeCategory,
				AppUserId = newUser.Id,
				AppUser = newUser,
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

			if (newUserResponse.Succeeded) 
			{ 
				await _userManager.AddToRoleAsync(newUser, UserRoles.Student);
			}
			_context.Students.Add(newStudent);
			_context.SaveChanges();


            var result = await _signInManager.PasswordSignInAsync(newUser, registerVM.Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return RedirectToAction("Index", "Home");
        }

		[HttpGet]
		public IActionResult RegisterDeaneryWorker()
		{
			var response = new RegisterDeaneryWorkerViewModel();
			return View(response);
		}

		[HttpPost]
		public async Task<IActionResult> RegisterDeaneryWorker(RegisterDeaneryWorkerViewModel registerDeaneryWorkerVM)
		{
			if (!ModelState.IsValid)
			{
				return View(registerDeaneryWorkerVM);
			}

			var user = await _userManager.FindByEmailAsync(registerDeaneryWorkerVM.EmailAddress);
			if (user != null)
			{
				TempData["Error"] = "This email address is already in use";
				return View(registerDeaneryWorkerVM);
			}


			var newUser = new AppUser()
			{
				RoleCategory = RoleCategory.Pracownik_Dziekanatu,
				FirstName = registerDeaneryWorkerVM.FirstName,
				LastName = registerDeaneryWorkerVM.LastName,
				PeselNumber = registerDeaneryWorkerVM.PeselNumber,
				BirthDate = registerDeaneryWorkerVM.BirthDate,
				Email = registerDeaneryWorkerVM.EmailAddress,
				UserName = registerDeaneryWorkerVM.EmailAddress,
			};

			var newDeaneryWorker = new DeaneryWorker()
			{
				AppUserId = newUser.Id,
				AppUser = newUser,
				PositionCategory = registerDeaneryWorkerVM.PositionCategory,
			};

            var newUserResponse = await _userManager.CreateAsync(newUser, registerDeaneryWorkerVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.DeaneryWorker);
            }
			_context.DeaneryWorkers.Add(newDeaneryWorker);
			_context.SaveChanges();

            return RedirectToAction("Index", "Dashboard");
        }


            [HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}

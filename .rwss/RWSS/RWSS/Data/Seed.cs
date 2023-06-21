using Microsoft.AspNetCore.Identity;
using RWSS.Data.Enum;
using RWSS.Models;
using System.Runtime.InteropServices;

namespace RWSS.Data
{
	public class Seed
	{
		public static void SeedData(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
				context.Database.EnsureCreated();

				if (!context.Events.Any())
				{
					context.Events.AddRange(new List<Event>()
					{
						new Event()
						{
							Title = "Oddanie Projektu",
							Description = "Oddanie aplikacji z projektu zespołowego na zaliczenie przedmiotu",
							EventCategory = EventCategory.Prezentacja,
						},

						new Event()
						{
							Title = "Zakończenie Roku",
							Description = "Zakończenie III roku studiów stacjonarnych, kierunek Informatyka na UMCS",
							EventCategory= EventCategory.Spotkanie,
						}
					});
					context.SaveChanges();
				}
			}
		}

		public static async Task SeedRolesAsync(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

/*				if (!await roleManager.RoleExistsAsync(UserRoles.Student))
				{
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Student));
				}

                if (!await roleManager.RoleExistsAsync(UserRoles.RWSSUser))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.RWSSUser));
                }

                if (!await roleManager.RoleExistsAsync(UserRoles.RWSSAdmin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.RWSSAdmin));
                }

                if (!await roleManager.RoleExistsAsync(UserRoles.DeaneryWorker))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.DeaneryWorker));
                }*/

				var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
				var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                string adminUserEmail = "adminRWSS@gmail.com";

				var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
				if (adminUser == null)
				{
					var newAdminUser = new AppUser()
					{
						UserName = "Admin RWSS",
						Email = adminUserEmail,
						EmailConfirmed = true,
						FirstName = "Paweł",
						LastName = "Paluch",
						PeselNumber = "123456789",
						RoleCategory = RoleCategory.Admin_RWSS,
						BirthDate = DateTime.Parse("01.06.1999"),
					};

					var newStudent = new Student()
					{
						AppUserId = newAdminUser.Id,
						AppUser = newAdminUser,
						IndexNumber = "123456",
						YearCategory = YearCategory.III,
						SemesterCategory = SemesterCategory.II,
						DegreeCourseCategory = DegreeCourseCategory.Informatyka,
						StudiesDegreeCategory = StudiesDegreeCategory.Licencjackie
					};

					await userManager.CreateAsync(newAdminUser, "zaq1@WSX");
					await userManager.AddToRoleAsync(newAdminUser, UserRoles.RWSSAdmin);

					context.Students.Add(newStudent);
					context.SaveChanges();
				}

				string adminUser2Email = "arwss9512@gmail.com";
                var adminUser2 = await userManager.FindByEmailAsync(adminUser2Email);
                if (adminUser2 == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "arwss9512@gmail.com",
						NormalizedUserName = "ARWSS9512@GMAIL.COM",
                        Email = adminUserEmail,
						NormalizedEmail = "ARWSS9512@GMAIL.COM",
                        EmailConfirmed = false,
                        FirstName = "Admin",
                        LastName = "Testowy",
                        PeselNumber = "999999999",
                        RoleCategory = RoleCategory.Admin_RWSS,
                        BirthDate = DateTime.Parse("01.07.1999"),
                    };

                    var newStudent = new Student()
                    {
                        AppUserId = newAdminUser.Id,
                        AppUser = newAdminUser,
                        IndexNumber = "999999",
                        YearCategory = YearCategory.III,
                        SemesterCategory = SemesterCategory.II,
                        DegreeCourseCategory = DegreeCourseCategory.Informatyka,
                        StudiesDegreeCategory = StudiesDegreeCategory.Licencjackie
                    };

                    await userManager.CreateAsync(newAdminUser, "zaq1@WSX");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.RWSSAdmin);

                    context.Students.Add(newStudent);
                    context.SaveChanges();
                }

                string deaneryWorkerEmail = "dziekan@gmail.com";
                var deaneryWorker = await userManager.FindByEmailAsync(deaneryWorkerEmail);
				
                if (deaneryWorker == null)
                {
                    var newUser = new AppUser()
                    {
                        UserName = "Pracownik dziekanatu",
                        Email = deaneryWorkerEmail,
                        EmailConfirmed = true,
                        FirstName = "Dziekan",
                        LastName = "Pro",
                        PeselNumber = "999999999",
                        RoleCategory = RoleCategory.Pracownik_Dziekanatu,
                        BirthDate = DateTime.Parse("01.06.1965"),
                    };

                    var newDeaneryWorker = new DeaneryWorker()
                    {
                        AppUserId = newUser.Id,
                        AppUser = newUser,
						PositionCategory = PositionCategory.Dziekan,
                    };

                    await userManager.CreateAsync(newUser, "zaq1@WSX");
                    await userManager.AddToRoleAsync(newUser, UserRoles.DeaneryWorker);

                    context.DeaneryWorkers.Add(newDeaneryWorker);
                    context.SaveChanges();
                }

                string deaneryWorker2Email = "dziekanpro@gmail.com";
                var deaneryWorker2 = await userManager.FindByEmailAsync(deaneryWorkerEmail);

                if (deaneryWorker2 == null)
                {
                    var newUser = new AppUser()
                    {
                        UserName = "dziekanpro@gmail.com",
                        NormalizedUserName = "DZIEKANPRO@GMAIL.COM",
                        Email = deaneryWorkerEmail,
                        NormalizedEmail = "DZIEKANPRO@GMAIL.COM",
                        EmailConfirmed = false,
                        FirstName = "ProPro",
                        LastName = "Dziekan",
                        PeselNumber = "999999999",
                        RoleCategory = RoleCategory.Pracownik_Dziekanatu,
                        BirthDate = DateTime.Parse("01.06.1950"),
                    };

                    var newDeaneryWorker = new DeaneryWorker()
                    {
                        AppUserId = newUser.Id,
                        AppUser = newUser,
                        PositionCategory = PositionCategory.Dziekan,
                    };

                    await userManager.CreateAsync(newUser, "zaq1@WSX");
                    await userManager.AddToRoleAsync(newUser, UserRoles.DeaneryWorker);

                    context.DeaneryWorkers.Add(newDeaneryWorker);
                    context.SaveChanges();
                }
            }
		}
	}
}

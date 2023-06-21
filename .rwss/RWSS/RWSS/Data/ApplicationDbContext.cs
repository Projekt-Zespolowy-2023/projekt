using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RWSS.Models;

namespace RWSS.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
		public DbSet<Event> Events { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<DeaneryWorker> DeaneryWorkers { get; set;}
	}
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RWSS.Converters;
using RWSS.Models;

namespace RWSS.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<TimeOnly>().HaveConversion<TimeOnlyConverter>();
        }
        public DbSet<Event> Events { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<DeaneryWorker> DeaneryWorkers { get; set;}
		public DbSet<Message> Messages { get; set; }
		public DbSet<Duty> Duties { get; set; }
		public DbSet<Assignment> Assignments { get; set; }
	}
}

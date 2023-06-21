using Microsoft.AspNetCore.Identity;
using RWSS.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace RWSS.Models
{
	public class AppUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PeselNumber { get; set; }
		public RoleCategory RoleCategory { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		[Display(Name = "Data Urodzenia")]
		public DateTime BirthDate { get; set; }
	}
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UniversityMgmtSystem.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Display(Name = "Full name")]
		public string FullName { get; set; }

		/*[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }*/

	}
}

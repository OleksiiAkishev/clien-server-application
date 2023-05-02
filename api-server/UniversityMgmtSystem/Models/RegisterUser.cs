using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UniversityMgmtSystemServerApi.Models
{
	public class RegisterUser : IdentityUser
	{
		/*[Required(ErrorMessage = "User name is required")]
		public string? UserName { get; set; }
		[EmailAddress]
		[Required(ErrorMessage = "Email is required")]
		public string? Email { get; set; }
		[Required(ErrorMessage = "Password is required")]
		public string? Password { get; set; }*/

	}
}

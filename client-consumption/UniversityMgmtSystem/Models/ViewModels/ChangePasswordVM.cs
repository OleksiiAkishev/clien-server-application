using System.ComponentModel.DataAnnotations;
namespace UniversityMgmtSystemClientConsuming.ViewModels
{
	public class ChangePasswordVM
	{
		[Display(Name = "Email address")]
		[Required(ErrorMessage = "Email address is required")]
		public string EmailAddress { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string CurrentPassword { get; set; }


		[Required]
		[DataType(DataType.Password)]

		public string NewPassword { get; set; }
		[Compare("NewPassword")]
		public string ConfirmPassword { get; set; }
	}
}

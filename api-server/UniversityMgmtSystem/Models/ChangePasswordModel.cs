using System.ComponentModel.DataAnnotations;

namespace UniversityMgmtSystemServerApi.Models
{
	public class ChangePasswordModel
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

		[DataType(DataType.Password)]
		[Compare("NewPassword")]
		public string ConfirmPassword { get; set; }
	}
}

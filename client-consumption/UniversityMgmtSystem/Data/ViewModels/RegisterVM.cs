using System.ComponentModel.DataAnnotations;

namespace UniversityMgmtSystem.Data.ViewModels
{
    public class RegisterVM
    {
		[Display(Name = "Full name")]
		[Required(ErrorMessage = "Full name address is required!")]

		public string FullName { get; set; }
		[Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required!")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

		[Required(ErrorMessage = "Confirm password is required!")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Passwords do not match!")]
		public string ConfirmPassword { get; set; }
	}
}
 
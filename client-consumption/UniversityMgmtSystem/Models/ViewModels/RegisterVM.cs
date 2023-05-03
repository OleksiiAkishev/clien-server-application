using System.ComponentModel.DataAnnotations;

namespace UniversityMgmtSystemClientConsuming.ViewModels
{
    public class RegisterVM
    {
		[Display(Name = "Full name")]
		[Required(ErrorMessage = "Full name address is required!")]

		public string FullName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", ErrorMessage = "Invalid pattern.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

		[Required(ErrorMessage = "Confirm password is required!")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Passwords do not match!")]
		public string ConfirmPassword { get; set; }
	}
}
 
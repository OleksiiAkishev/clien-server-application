using System.ComponentModel.DataAnnotations;

namespace UniversityMgmtSystemServerApi.ViewModels
{
    public class RegisterVM
    {
		[Display(Name = "Full name")]
		[Required(ErrorMessage = "Full name address is required!")]

		public string FullName { get; set; }
		[Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required!")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
	}
}
 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniversityMgmtSystem.Data;
using UniversityMgmtSystem.Data.Static;
using UniversityMgmtSystem;
using UniversityMgmtSystem.Models;
using UniversityMgmtSystemServerApi.Models;
using UniversityMgmtSystemServerApi.ViewModels;

namespace UniversityMgmtSystem.Controllers
{

	[Route("api/[controller]")]
	public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<IdentityUser> _roleManager;
        private readonly AppDbContext _appDbContext;
		private readonly IConfiguration _configuration;

		public AccountController(UserManager<ApplicationUser> userManager,
            AppDbContext appDbContext, IConfiguration configuration) 
        {
            _userManager = userManager;
            //_roleManager = roleManager;
            _appDbContext = appDbContext;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /*public IActionResult Login()
        {
            var response = new LoginVM();
            return View(response);
        }*/


        [HttpGet]
        public async Task<IActionResult> GetDummy() 
        {
            return Ok("Good result");
        }


        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterVM registerUser) 
        {
            //Check if user exist in the DB
            var userFromDb = await _userManager.FindByEmailAsync(registerUser.Password);
            if (userFromDb != null) 
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "User with these credentials already exist!" });
            }

			//If not exist, Add a new user to DB
			var newUser = new ApplicationUser()
			{
				FullName = registerUser.FullName,
				Email = registerUser.Email,
                //SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.Email
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerUser.Password);
			if (newUserResponse.Succeeded)
			{
				await _userManager.AddToRoleAsync(newUser, UserRoles.Student);
                return StatusCode(StatusCodes.Status201Created,
                        new Response { Status = "Success", Message = "User was created successfully!" });
			}
			return StatusCode(StatusCodes.Status500InternalServerError,
					new Response { Status = "Error", Message = "User Failed to Create!" });
        }

		/*public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) 
            {
                return View(loginVM);
            }
            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null) 
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
				TempData["Error"] = "Wrong credentials. Please, try again!";
				return View(loginVM);
			}
            TempData["Error"] = "Wrong credentials. Please, try again!";
            return View(loginVM);
        }*/

		/*public IActionResult Register()
		{
			var response = new RegisterVM();
			return View(response);
		}

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null) 
            {
				TempData["Error"] = "This email is in use already!";
                return View(registerVM);
			}
            var newUser = new ApplicationUser()
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded) 
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Student);
            }
            return View("RegisterCompleted");
        }

        [HttpPost]
        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync(); 
            return RedirectToAction("Login", "Account");
        }*/
	}
}

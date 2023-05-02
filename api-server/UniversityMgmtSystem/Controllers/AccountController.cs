using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniversityMgmtSystem.Data;
using UniversityMgmtSystem.Data.Static;
using UniversityMgmtSystem;
using UniversityMgmtSystem.Models;
using UniversityMgmtSystemServerApi.Models;
using UniversityMgmtSystemServerApi.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UniversityMgmtSystem.Controllers
{

	[Route("api/[controller]")]
	public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<IdentityUser> _roleManager;
        private readonly AppDbContext _appDbContext;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,
            AppDbContext appDbContext, IConfiguration configuration) 
        {
            _userManager = userManager;
            //_roleManager = roleManager;
            _appDbContext = appDbContext;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login([FromBody] LoginModel loginUser)
        {
			
			var user = await _userManager.FindByEmailAsync(loginUser.EmailAddress);
			if (user != null)
			{
				var passwordCheck = await _userManager.CheckPasswordAsync(user, loginUser.Password);
				if (passwordCheck)
				{
					var userRoles = await _userManager.GetRolesAsync(user);

					var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				};

					foreach (var userRole in userRoles)
					{
						authClaims.Add(new Claim(ClaimTypes.Role, userRole));
					}

					var token = GetToken(authClaims);

					return Ok(new
					{
						token = new JwtSecurityTokenHandler().WriteToken(token),
						expiration = token.ValidTo
					});
				}
				return Unauthorized();
			}
			return Unauthorized();
		}


        [HttpGet]
		[Route("GetDummy")]
		public async Task<IActionResult> GetDummy() 
        {
            return Ok("Good result");
        }


        [HttpPost]
		[Route("Register")]
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
                UserName = registerUser.Email,
				EmailConfirmed = true

			};

            var newUserResponse = await _userManager.CreateAsync(newUser, registerUser.Password.Trim());
			if (newUserResponse.Succeeded)
			{
				await _userManager.AddToRoleAsync(newUser, UserRoles.Student);
                return StatusCode(StatusCodes.Status201Created,
                        new Response { Status = "Success", Message = "User was created successfully!" });
			}
			return StatusCode(StatusCodes.Status500InternalServerError,
					new Response { Status = "Error", Message = "User Failed to Create!" });
        }

		private JwtSecurityToken GetToken(List<Claim> authClaims)
		{
			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));

			var token = new JwtSecurityToken(
				issuer: Configuration["JWT:ValidIssuer"],
				audience: Configuration["JWT:ValidAudience"],
				expires: DateTime.Now.AddHours(3),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				);

			return token;
		}

		
		/*[HttpPost]
		[Route("Logout")]
		public async Task<IActionResult> Logout()
		{


			_signInManager.SignOutAsync();
			return Ok();
		}
		*/
		[HttpPost]
		[Route("ChangePassword")]
		public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordVM changePasswordUser)

		{
			var checkUser = await _userManager.FindByNameAsync(changePasswordUser.EmailAddress);
			if(checkUser==null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Response
				{
					Status = "Error",
					Message = "UserName not Found"
				});
			}
		
			ApplicationUser user = new ApplicationUser
			{
				UserName = checkUser.UserName,
				Email = checkUser.Email,
			};
			if (!await _userManager.CheckPasswordAsync(user, changePasswordUser.CurrentPassword.Trim()))
			{
				return StatusCode(StatusCodes.Status406NotAcceptable, new Response
				{
					Status = "Error",
					Message = "CurrentPassword not Found"
				});

			}

		 var result =  await _userManager.ChangePasswordAsync(user, changePasswordUser.CurrentPassword, changePasswordUser.NewPassword);

			if(result.Succeeded) {


				return StatusCode(StatusCodes.Status202Accepted, new Response
				{
					Status = "Error",
					Message = "Succesfully Changed"
				});
			}
			return StatusCode(StatusCodes.Status406NotAcceptable, new Response
			{
				Status = "Error",
				Message = "Password not changed"
			});
		}
	}
}

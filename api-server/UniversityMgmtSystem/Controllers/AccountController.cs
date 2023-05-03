using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniversityMgmtSystem.Data;
using UniversityMgmtSystem.Data.Static;
using UniversityMgmtSystemServerApi.Models;
using UniversityMgmtSystemServerApi.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UniversityMgmtSystemServerApi.Controllers
{

	[Route("api/[controller]")]
	public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _appDbContext;

        public AccountController(UserManager<ApplicationUser> userManager,
            AppDbContext appDbContext, IConfiguration configuration)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginUser)
        {
            var user = await _userManager.FindByEmailAsync(loginUser.Email);
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
                    var identity = new ClaimsIdentity(authenticationType: "JWT");
                    identity.AddClaims(authClaims);
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

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerUser)
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

        [HttpPost]
		[Route("ChangePassword")]
		public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordModel changePasswordUser)
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
            if (!await _userManager.CheckPasswordAsync(checkUser, changePasswordUser.CurrentPassword.Trim()))
			{
				return StatusCode(StatusCodes.Status406NotAcceptable, new Response
				{
					Status = "Error",
					Message = "CurrentPassword not Found"
				});
			}
		 var result = await _userManager.ChangePasswordAsync(checkUser, changePasswordUser.CurrentPassword, changePasswordUser.NewPassword);
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
    }
}

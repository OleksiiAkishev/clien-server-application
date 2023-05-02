using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using UniversityMgmtSystemClientConsuming.Models;
using UniversityMgmtSystemClientConsuming.ViewModels;

namespace UniversityMgmtSystem.Controllers
{
	public class AccountController : Controller
	{
		private readonly static HttpClient httpClient = new();
		public AccountController(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		private IConfiguration Configuration { get; }
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Login(User user)
		{
			if (user.Email == null || user.Password == null)
			{
				return View("Login");
			}
			var request = new HttpRequestMessage(HttpMethod.Post, Configuration.GetValue<string>("WebAPIBaseUrl") + "/Account");
			var serializedUser = JsonConvert.SerializeObject(user);
			request.Content = new StringContent(serializedUser);
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var response = httpClient.Send(request);
			if (response.IsSuccessStatusCode)
			{
				var token = response.Content.ReadAsStringAsync().Result;
				JWT jwt = JsonConvert.DeserializeObject<JWT>(token);
				HttpContext.Session.SetString("token", jwt.Token);
				HttpContext.Session.SetString("UserName", user.Email);

				return RedirectToAction("Index", "Home");
			}
			ViewBag.Message = "Invalid Username or Password";
			return View("Login");
		}

		public IActionResult Register()
		{
			var response = new RegisterVM();
			return View(response);
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterVM registerVM)
		{
			using var httpClient = new HttpClient();
			var content = new StringContent(JsonConvert.SerializeObject(registerVM), Encoding.UTF8, "application/json");
			var response = await httpClient.PostAsync("https://localhost:7003/api/Account", content);

			if (response.StatusCode == HttpStatusCode.Created)
			{
				TempData["Success"] = "Registration was successful! Please log in.";
				return View("RegisterCompleted");
			}
			else if (response.StatusCode == HttpStatusCode.Forbidden)
			{
				TempData["Error"] = "User with these credentials already exist!";
			}
			else
			{
				TempData["Error"] = "An error occurred while registering the user. Please try again later.";
			}

			return View(registerVM);
		}

		/*[HttpPost]
        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync(); 
            return RedirectToAction("Login", "Account");
        }*/
	}
}

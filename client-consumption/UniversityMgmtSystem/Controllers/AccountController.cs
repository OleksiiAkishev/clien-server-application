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
        public async Task<IActionResult> Login(User user)
        {
            // Create an instance of HttpClient
            using var httpClient = new HttpClient();

            // Send a POST request to the API endpoint
            var response = await httpClient.PostAsJsonAsync("https://localhost:7003/api/Account/Login", user);

            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as JSON and get the token and expiration date
                var responseContent = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                var token = responseContent["token"];
                var expiration = responseContent["expiration"];

                // Store the token and expiration date in a cookie or local storage




                // Redirect to the home page or another page
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Display an error message
                TempData["Error"] = "Invalid email address or password.";
                return View();
            }
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
			var response = await httpClient.PostAsync("https://localhost:7003/api/Account/Register", content);

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

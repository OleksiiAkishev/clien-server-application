
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using UniversityMgmtSystemClientConsuming.Models;
using UniversityMgmtSystemClientConsuming.ViewModels;

namespace UniversityMgmtSystemClientConsuming.Controllers
{
	public class AccountController : Controller
	{
        public AccountController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
		{
			Configuration = configuration;
            HttpClientFactory = httpClientFactory;
		}
		private IConfiguration Configuration { get; }

        private IHttpClientFactory HttpClientFactory { get; }

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

        [HttpPost]
        public IActionResult Login(LoginVM user)
        {
            if (user.Email == null || user.Password == null)
            {
                return View("Login");
            }
            using var httpClient = HttpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, Configuration.GetValue<string>("BaseAddress") + "/Account/Login");
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
                HttpContext.Session.SetString("Email", user.Email);

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
            string serverAddressApi = "https://localhost:7003/api/Account/Register";
            var content = new StringContent(JsonConvert.SerializeObject(registerVM), Encoding.UTF8, "application/json");

            using var httpClient = HttpClientFactory.CreateClient();

            var response = await httpClient.PostAsync(serverAddressApi, content);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                ViewData["Success"] = "Registration was successful! Please log in.";
                return View("RegisterCompleted");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                ViewData["Error"] = "User with these credentials already exist!";
            }
            else
            {
                ViewData["Error"] = "An error occurred while registering the user. Please try again later.";
            }
            return View("Register", registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
		public async Task<IActionResult> ChangePassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword (ChangePasswordVM changePasswordVM )
		{
			if(changePasswordVM.EmailAddress == null)
			{
				ViewData["Error"] = "Empty Values";
				return View();
			}
			using var httpClient= new HttpClient();
			var content = new StringContent(JsonConvert.SerializeObject(changePasswordVM), Encoding.UTF8, "application/json");
			var response = await httpClient.PostAsync("https://localhost:7003/api/Account/ChangePassword", content);
			if(!response.IsSuccessStatusCode)
			{
                var statuscode = await response.Content.ReadAsStringAsync();
                var statusmessgae = JsonConvert.DeserializeObject<ApiStatus>(statuscode);
                ViewData["Error"]=	statusmessgae.Message;
				return View();
			}
			return View("ChangePasswordCompleted");
		}
	}
}


//N6n7C$6*Fibe
//DzCa0^L881$J
//shailesh@gmail.com
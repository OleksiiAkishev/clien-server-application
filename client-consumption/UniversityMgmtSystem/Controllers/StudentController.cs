using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace UniversityMgmtSystemClientConsuming.Controllers
{
	public class StudentController : Controller
	{
		public IActionResult DashBoard()
		{

			return View();
		}

		public async Task<IActionResult> GetCourse()
		{
			List<Course> courses = new List<Course>();
			using (HttpClient httpClient = new HttpClient())
			{

				var response = await httpClient.GetAsync("https://localhost:7003/api/Course/GetCourses");

				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					courses = JsonConvert.DeserializeObject<List<Course>>(content);
					return View(courses);
				}

			}
			return BadRequest();
			return View();
		}
		public async Task<IActionResult> EnrollCourse(int id)
		{
			return View("DashBoard");
		}
	}
}

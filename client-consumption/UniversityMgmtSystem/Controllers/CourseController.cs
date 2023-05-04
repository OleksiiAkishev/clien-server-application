using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using UniversityMgmtSystemClientConsuming.Models;

namespace UniversityMgmtSystemClientConsuming.Controllers
{
	public class CourseController : Controller
	{
		private readonly static HttpClient _httpClient= new HttpClient();
		public async Task<IActionResult> Index()
		{
			List<Course> courses = new List<Course>();
			using(HttpClient httpClient = new HttpClient())
			{

				   var response=  await httpClient.GetAsync("https://localhost:7003/api/Course/GetCourses");

			if(response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					courses= JsonConvert.DeserializeObject<List<Course>>(content);
					return View(courses);
				}

			}
			return BadRequest();
		}

		[HttpGet]
		public IActionResult CreateCourse ()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateCourse(Course course)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7003/api/Course/CreateCourse");
			var content= new StringContent(JsonConvert.SerializeObject(course));
		
			request.Content = content;
			request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
			request.Content.Headers.ContentType=new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
			var response = await _httpClient.SendAsync(request);
				if (!response.IsSuccessStatusCode)
				{
					var statuscode = await response.Content.ReadAsStringAsync();
					var statusmessgae = JsonConvert.DeserializeObject<ApiStatus>(statuscode);
					ViewData["Error"] = statusmessgae.Message;
					return View();
				}

				return RedirectToAction("Index");
		
		}
		[HttpGet]
		public async Task<IActionResult> UpdateCourse(int id)
		{
			using(HttpClient  client = new HttpClient())
			{
				var response =await client.GetAsync($"https://localhost:7003/api/Course/GetCourseById/{id}") ;
				if(response.IsSuccessStatusCode)
				{
					Course course = new Course();
					String content= await response.Content.ReadAsStringAsync();

					course = JsonConvert.DeserializeObject<Course>(content);

					return View(course);	
				}


			}

			return BadRequest("No Course Found for id:"+ id);
		}
		[HttpPost]
		public async Task<IActionResult> UpdateCourse(Course course)
		{
			using (HttpClient httpClient = new HttpClient())
			{
				ApiStatus apiStatus= new ApiStatus();
				var response = await httpClient.PostAsJsonAsync("https://localhost:7003/api/Course/UpdateCourse", course);
				if (!response.IsSuccessStatusCode) 
				 { 
					var content = await response.Content.ReadAsStringAsync();

					apiStatus = JsonConvert.DeserializeObject<ApiStatus>(content);
					ViewData["Error"] = apiStatus.Message;
					return View();	


					
				
				 }


				return View("Index");
			}
		}
		[HttpGet]
		public async Task<IActionResult> DeleteCourse(int id)
		{

			using (HttpClient client = new HttpClient())
			{
				var response = await client.GetAsync("https://localhost:7003/api/Course/GetCourseById/" + id);
				if (response.IsSuccessStatusCode)
				{
					Course course = new Course();
					String content = await response.Content.ReadAsStringAsync();

					course = JsonConvert.DeserializeObject<Course>(content);

					return View(course);
				}
				

			}

			return BadRequest("No Course Found for id:" + id);
		}
		[HttpPost]
		public async Task<IActionResult> DeleteCourse(Course course )
		{
			ApiStatus apiStatus = new ApiStatus();
			using (HttpClient httpClient = new HttpClient()) 
			{
				var response = await httpClient.DeleteAsync("https://localhost:7003/api/Course/DeleteCourse/" + course.CourseId);
				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					apiStatus = JsonConvert.DeserializeObject<ApiStatus>(content);
					ViewData["Error"] = apiStatus.Message;
					return View();


				}
				return BadRequest("No Course Found for id:" + course.CourseId);

			}



		
		}


	}
}

//slotnum 
//uniqueslotnum and range
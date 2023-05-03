using Microsoft.AspNetCore.Mvc;

namespace UniversityMgmtSystemClientConsuming.Controllers
{
	public class CourseController : Controller
	{
		public IActionResult Index()
		{

			return View();
		}

	}
}

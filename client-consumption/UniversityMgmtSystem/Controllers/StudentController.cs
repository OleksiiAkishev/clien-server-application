using Microsoft.AspNetCore.Mvc;

namespace UniversityMgmtSystemClientConsuming.Controllers
{
	public class StudentController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

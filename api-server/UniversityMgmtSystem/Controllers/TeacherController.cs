using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityMgmtSystem.Data;
using UniversityMgmtSystemServerApi.Models;

namespace UniversityMgmtSystemServerApi.Controllers
{
	[Route("api/[controller]")]
	public class TeacherController : Controller
	{

		AppDbContext _db;
		public TeacherController( AppDbContext appDbContext)
		{
			_db = appDbContext;
		}
   


		[HttpGet]
		[Route("GetTeacher")]
		public async Task<List<Teacher>> GetTeachers()
		{
			return await _db.Teachers.ToListAsync();
		}

		[HttpPost]
		[Route("CreateTeacher")]
		public async Task<IActionResult> CreateTeacher( Teacher teacher)
		{

			if(teacher == null)
			{
				return StatusCode( StatusCodes.Status404NotFound,
					new Response
					{
						Status="Error",
						Message="Teacher is null"
					}
					);

			}

			await _db.Teachers.AddAsync(teacher);
			await _db.SaveChangesAsync();
			return StatusCode(StatusCodes.Status200OK);
		}
		[HttpPost]
		[Route("UpdateTeacher")]
		public async Task<IActionResult> UpdateTeacher(Teacher teacher)
		{

			var editTeacher = await _db.Teachers.Where(t => t.TeacherId == teacher.TeacherId).FirstOrDefaultAsync();
			if(editTeacher == null)
			{
				return StatusCode(StatusCodes.Status404NotFound,
					new Response
					{
						Status="Error",
						Message="No Teacher found similiar"
					});
			}

			 editTeacher.TeacherName = teacher.TeacherName;
			await _db.SaveChangesAsync();

			return StatusCode(StatusCodes.Status200OK);
		

		}

	}
}

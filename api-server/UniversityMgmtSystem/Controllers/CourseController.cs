using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityMgmtSystem.Data;
using UniversityMgmtSystemServerApi.Models;

namespace UniversityMgmtSystemServerApi.Controllers
{
	[Route("api/[controller]")]
	public class CourseController : Controller
	{

		private AppDbContext _db;
		int ClassroomCounter = 0, i = 1;
		int classRoomLength = 5;
		int dayCounter = 1;
		int slotnum = 1;
		public CourseController(AppDbContext db)
		{
			_db = db;
		}


		[HttpGet]
		[Route("GetCourses")]
		public async Task<IEnumerable<Course>> GetCourses()
		{
			return await _db.Courses.ToListAsync();
		}


		[HttpPost]
		[Route("CreateCourse")]
		public async Task<IActionResult> CreateCourse(Course course)
		{

			if (ModelState.IsValid)
			{


				if (course.NumOfClassPerWeek > 3 || course.NumOfSlot > 3)
				{
					
					return StatusCode(StatusCodes.Status416RequestedRangeNotSatisfiable,new Response
					{
						Status="Error",
						Message= "Num of classes and slot can more than 3 "
					});
				}
				await _db.Courses.AddAsync(course);
				await _db.SaveChangesAsync();
				while (i < course.NumOfClassPerWeek && ClassroomCounter < 2 && slotnum < 6)
				{

					Day days = await _db.Days.Where(day => day.DayNum == dayCounter).FirstOrDefaultAsync();
					days.ClassRooms = await _db.ClassRooms.Where(classroom => classroom.DayId == days.DayId).ToListAsync();
					classRoomLength = days.ClassRooms.Count();
					int classroomId = days.ClassRooms[ClassroomCounter].ClassRoomId;
					Course createdCourse = await _db.Courses.Where(cou => cou.CourseName == course.CourseName).FirstOrDefaultAsync();

					if (await _db.Slots.Where(s => s.SlotNum == slotnum && s.ClassRoomId == classroomId).FirstOrDefaultAsync() == null)
					{
						for (int j = 0; j < course.NumOfSlot; j++)
						{

							if (slotnum > 5)
							{
								i--;
								slotnum--;
								ClassroomCounter++;
								break;
							}
							Slot slot = new Slot()
							{
								SlotNum = slotnum + j,
								CourseId = createdCourse.CourseId,
								ClassRoomId = days.ClassRooms[0].ClassRoomId
							};

							await _db.Slots.AddAsync(slot);
							await _db.SaveChangesAsync();


						};
						i++;
						slotnum++;
						dayCounter = i;
					}

					if (ClassroomCounter >= classRoomLength)
					{
						dayCounter++;

					}
					else if (dayCounter > 5)
					{
						

						return StatusCode(StatusCodes.Status416RequestedRangeNotSatisfiable,
							new Response
							{
								Status="Error",
								Message= "Create more ClassRooms"
							});

					}




				}

				return StatusCode(StatusCodes.Status200OK);
			}

			return StatusCode(StatusCodes.Status406NotAcceptable,
				new Response { 
				
				Status="Eroor",
				Message="Invliad Input format"
				});

		}
	

		[HttpPost]
		[Route("UpdateCourse")]
		public async Task<IActionResult> UpdateCourse(Course course)
		{
			var editCourse = await _db.Courses.Where(c => c.CourseId == course.CourseId).FirstOrDefaultAsync();
			editCourse.CourseName = course.CourseName.Trim();
			editCourse.NumOfSlot = course.NumOfSlot;
			editCourse.NumOfClassPerWeek = course.NumOfClassPerWeek;
			var deleteSlots = await _db.Slots.Where(s => s.CourseId == editCourse.CourseId).ToListAsync();
			foreach (var delslot in deleteSlots)
			{
				_db.Slots.Remove(delslot);

			}	
		  await	_db.SaveChangesAsync();
			while (i < course.NumOfClassPerWeek && ClassroomCounter < 2 && slotnum < 6)
			{

				Day days = await _db.Days.Where(day => day.DayNum == dayCounter).FirstOrDefaultAsync();
				days.ClassRooms = await _db.ClassRooms.Where(classroom => classroom.DayId == days.DayId).ToListAsync();
				classRoomLength = days.ClassRooms.Count();
				int classroomId = days.ClassRooms[ClassroomCounter].ClassRoomId;
				Course createdCourse = await _db.Courses.Where(cou => cou.CourseName == course.CourseName).FirstOrDefaultAsync();

				if ( await _db.Slots.Where(s => s.SlotNum == slotnum && s.ClassRoomId == classroomId).FirstOrDefaultAsync() == null)
				{
					for (int j = 0; j < course.NumOfSlot; j++)
					{

						if (slotnum > 5)
						{
							i--;
							slotnum--;
							ClassroomCounter++;
							break;
						}
						Slot slot = new Slot()
						{
							SlotNum = slotnum + j,
							CourseId = createdCourse.CourseId,
							ClassRoomId = days.ClassRooms[0].ClassRoomId
						};

					  await	_db.Slots.AddAsync(slot);
					  await	 _db.SaveChangesAsync();


					};
					i++;
					slotnum++;
					dayCounter = i;
				}

				if (ClassroomCounter >= classRoomLength)
				{
					dayCounter++;

				}
				else if (dayCounter > 5)
				{
					return StatusCode(StatusCodes.Status416RequestedRangeNotSatisfiable,
							new Response
							{
								Status = "Error",
								Message = "Create more ClassRooms"
							});

				}




			}

			return StatusCode(StatusCodes.Status200OK);


		}

		[HttpGet]
		[Route("GetCourseById/{id}")]
		public async Task<Course> GetCourseById(int id)
		{
			Course course = _db.Courses.FirstOrDefault(c => c.CourseId == id);


			return course;
		}

		[HttpDelete]
		[Route("DeleteCourse/{id}")]
		public async Task<IActionResult> DeleteCourse(int id)
		{
			var deleteCourse = await _db.Courses.Where(c => c.CourseId == id).FirstOrDefaultAsync();
			if(deleteCourse == null) {

				return StatusCode(StatusCodes.Status404NotFound,
					new Response
					{
						Status="Error",
						Message="No Course fount at id: "+id

					});
			
			
			}
			_db.Courses.Remove(deleteCourse);
			var deleteSlots = await _db.Slots.Where(s => s.CourseId == deleteCourse.CourseId).ToListAsync();
			foreach (var delslot in deleteSlots)
			{
			 	_db.Slots.Remove(delslot);

			}
		    await _db.SaveChangesAsync();



			return StatusCode(StatusCodes.Status200OK);
		}

	}
}

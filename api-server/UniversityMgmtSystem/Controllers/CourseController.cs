using Microsoft.AspNetCore.Mvc;
using UniversityMgmtSystem.Data;
using UniversityMgmtSystemServerApi.Models;

namespace UniversityMgmtSystemServerApi.Controllers
{
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


		public IActionResult Index()
		{
			List<Course> courses = _db.Courses.ToList();
			return View(courses);
		}

		public async Task<IEnumerable<Course>> GetCourses()
		{
			return _db.Courses.ToList();
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{

			return View();

		}
		[HttpPost]
		public async Task<IActionResult> Create(Course course)
		{

			if (ModelState.IsValid)
			{


				if (course.NumOfClassPerWeek > 3 || course.NumOfSlot > 3)
				{
					ViewData["Error"] = "Num of classes and slot can more than 3 ";
					return View();
				}
				_db.Courses.Add(course);
				_db.SaveChanges();
				while (i < course.NumOfClassPerWeek && ClassroomCounter < 2 && slotnum < 6)
				{

					Day days = _db.Days.Where(day => day.DayNum == dayCounter).FirstOrDefault();
					days.ClassRooms = _db.ClassRooms.Where(classroom => classroom.DayId == days.DayId).ToList();
					classRoomLength = days.ClassRooms.Count();
					int classroomId = days.ClassRooms[ClassroomCounter].ClassRoomId;
					Course createdCourse = _db.Courses.Where(cou => cou.CourseName == course.CourseName).FirstOrDefault();

					if (_db.Slots.Where(s => s.SlotNum == slotnum && s.ClassRoomId == classroomId).FirstOrDefault() == null)
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

							_db.Slots.Add(slot);
							_db.SaveChanges();


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
						ViewData["Error"] = "Create more ClassRooms";

						return View();

					}




				}

				return RedirectToAction("Index");
			}

			return View();

		}
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			Course course = _db.Courses.Where(c => c.CourseId == id).FirstOrDefault();
			return View(course);
		}


		[HttpPost]
		public async Task<IActionResult> Edit(Course course)
		{
			var editCourse = _db.Courses.Where(c => c.CourseId == course.CourseId).FirstOrDefault();
			editCourse.CourseName = course.CourseName.Trim();
			editCourse.NumOfSlot = course.NumOfSlot;
			editCourse.NumOfClassPerWeek = course.NumOfClassPerWeek;
			var deleteSlots = _db.Slots.Where(s => s.CourseId == editCourse.CourseId).ToList();
			foreach (var delslot in deleteSlots)
			{
				_db.Slots.Remove(delslot);

			}	
			_db.SaveChanges();
			while (i < course.NumOfClassPerWeek && ClassroomCounter < 2 && slotnum < 6)
			{

				Day days = _db.Days.Where(day => day.DayNum == dayCounter).FirstOrDefault();
				days.ClassRooms = _db.ClassRooms.Where(classroom => classroom.DayId == days.DayId).ToList();
				classRoomLength = days.ClassRooms.Count();
				int classroomId = days.ClassRooms[ClassroomCounter].ClassRoomId;
				Course createdCourse = _db.Courses.Where(cou => cou.CourseName == course.CourseName).FirstOrDefault();

				if (_db.Slots.Where(s => s.SlotNum == slotnum && s.ClassRoomId == classroomId).FirstOrDefault() == null)
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

						_db.Slots.Add(slot);
						_db.SaveChanges();


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
					ViewData["Error"] = "Create more ClassRooms";

					return View();

				}




			}

			return View("Index");


		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			Course course = _db.Courses.FirstOrDefault(c => c.CourseId == id);


			return View(course);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(Course course)
		{
			var editCourse = _db.Courses.Where(c => c.CourseId == course.CourseId).FirstOrDefault();
			editCourse.CourseName = course.CourseName.Trim();
			editCourse.NumOfSlot = course.NumOfSlot;
			editCourse.NumOfClassPerWeek = course.NumOfClassPerWeek;
			var deleteSlots = _db.Slots.Where(s => s.CourseId == editCourse.CourseId).ToList();
			foreach (var delslot in deleteSlots)
			{
				_db.Slots.Remove(delslot);

			}
			_db.SaveChanges();



			return RedirectToAction("Index");
		}

	}
}

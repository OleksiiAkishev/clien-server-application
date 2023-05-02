using System.ComponentModel.DataAnnotations;

namespace UniversityMgmtSystemServerApi.Models
{
	public class Course
	{
		[Key]
		public int CourseId { get; set; }
		public string CourseName { get; set; }
		public int NumOfSlot { get; set; }

		public int NumOfClassPerWeek { get; set; }	

		public int TeacherId { get; set; }
		public Teacher? Teacher { get; set; }
		public string AssigmentFile { get; set; }
		public List<StudentCourse>? studentCourses = new List<StudentCourse>();







	}
}

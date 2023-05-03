using Microsoft.AspNetCore.Mvc.ModelBinding;
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
		public string? AssigmentFile { get; set; }
		public List<StudentCourse>? studentCourses = new List<StudentCourse>();
        public List<Student> Students { get; } = new();
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }



    }
}

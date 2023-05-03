using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UniversityMgmtSystemServerApi.Models
{
	public class Student
	{
		[Key]
		public int StudentId { get; set; }

		public string StudentName { get; set; }
		
		public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }

        public int ContactNo { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }


        public List<StudentCourse> StudentCourses = new List<StudentCourse>();
        public List<Course> Courses { get; } = new();

    }
}

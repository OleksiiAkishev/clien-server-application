using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

	public class Student
	{
		[Key]
		public int StudentId { get; set; }

		public string StudentName { get; set; }
		
		public DateTime? DateOfBirth { get; set; }

		public List<StudentCourse>? studentCourses = new List<StudentCourse>(); 

	}


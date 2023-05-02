namespace UniversityMgmtSystemServerApi.Models
{
	public class Teacher
	{
		public int TeacherId { get; set; }	
		public string TeacherName { get; set;}
		public List<Course> Courses { get; set; } = new List<Course>();

		
	}
}

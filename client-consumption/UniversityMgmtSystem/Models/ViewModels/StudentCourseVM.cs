using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityMgmtSystemClientConsuming.ViewModels
{
	public class StudentCourseVM
	{
		[Key]
		public int StudentCourseId { get; set; }

		[ForeignKey("Student")]
		public int StudentId { get; set; }
		//public Student Student { get; set; }

		[ForeignKey("Course")]
		public int CouserId { get; set; }	
		//public Course Course { get; set;}


	}
}

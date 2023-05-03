using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using UniversityMgmtSystem.Data;
using UniversityMgmtSystemServerApi.Models;

namespace UniversityMgmtSystemServerApi.Controllers
{
    public class EnrollmentController : Controller
    {
        StudentCourse studentCourse = new StudentCourse();
        Student student = new Student();
        Course course = new Course();
        private AppDbContext appDbContext;

        public EnrollmentController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult GetCourses(int departmentId)
        {
            var courses = appDbContext.Courses.Where(c => c.DepartmentId == departmentId).ToList();

            return Json(courses);

        }
        [HttpGet]
        public IActionResult Enroll()
        {
            var departments = appDbContext.Departments.ToList();
            ViewBag.Departments = new SelectList(departments, "Id", "Name");

            var students = appDbContext.Students.ToList();
            ViewBag.Students = new SelectList(students, "StudentId", "StudentName");


            var courses = appDbContext.Courses.ToList();
            ViewBag.Courses = new SelectList(courses, "Id", "Name");

            return View();
        }
        [HttpPost]
        public IActionResult Enroll(StudentCourse studentCourse)
        {
            //appDbContext.Students.Add(student);
            //appDbContext.Courses.Add(course);
            appDbContext.Add(studentCourse);
            appDbContext.SaveChanges();
            ViewBag.Message = "Enrollment successful!";
            return View();
        }
    }
}

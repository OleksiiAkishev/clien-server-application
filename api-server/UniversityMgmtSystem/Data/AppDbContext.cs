using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniversityMgmtSystem.Models;
using UniversityMgmtSystemServerApi.Models;

namespace UniversityMgmtSystem.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Day>Days { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }



    }
}

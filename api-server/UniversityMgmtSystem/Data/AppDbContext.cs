using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Xml;
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
        public DbSet<Department> Departments { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<TimeTable>().HasData(


                new TimeTable
                {
                    TimeTableId = 1,
                    TimeTableName = "1st semester"
                }

                );

            builder.Entity<Day>().HasData(

            new Day
            {
                DayId = 1,
                DayNum = 1,
                TimeTableId = 1,

            },
            new Day
            {
                DayId = 2,
                DayNum = 2,
                TimeTableId = 1,

            },
              new Day
              {
                  DayId = 3,
                  DayNum = 3,
                  TimeTableId = 1,

              }
                ,
               new Day
               {
                   DayId = 4,
                   DayNum = 4,
                   TimeTableId = 1,
               },
                new Day
                {
                    DayId = 5,
                    DayNum = 5,
                    TimeTableId = 1,
                });



            builder.Entity<ClassRoom>().HasData(


                new ClassRoom
                {
                    ClassRoomId = 1,
                    ClassroomName = "A101",
                    DayId = 1

                },

                new ClassRoom
                {
                    ClassRoomId = 2,
                    ClassroomName = "A102",
                    DayId = 1
                },

                new ClassRoom
                {
                    ClassRoomId = 3,
                    ClassroomName = "A101",
                    DayId = 2
                },

                new ClassRoom
                {
                    ClassRoomId = 4,
                    ClassroomName = "A102",
                    DayId = 2
                },

                new ClassRoom
                {
                    ClassRoomId = 5,
                    ClassroomName = "A101",
                    DayId = 3
                },

                new ClassRoom
                {
                    ClassRoomId = 6,
                    ClassroomName = "A102",
                    DayId = 3
                },
                new ClassRoom
                {
                    ClassRoomId = 7,
                    ClassroomName = "A101",
                    DayId = 4
                },
                new ClassRoom
                {
                    ClassRoomId = 8,
                    ClassroomName = "A102",
                    DayId = 4
                },
                new ClassRoom
                {
                    ClassRoomId = 9,
                    ClassroomName = "A101",
                    DayId = 5
                },
                new ClassRoom
                {
                    ClassRoomId = 10,
                    ClassroomName = "A102",
                    DayId = 5
                }
                );




            base.OnModelCreating(builder);
        }


    }
}



using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniversityMgmtSystemServerApi.Models;


    public class Slot
    {
        [Key]
        public int SlotID { get; set; }
        [Range(1, 6)]
        public int SlotNum { get; set; }


        [ForeignKey("Course")]
        public int CourseId { get; set; }   
        public Course Courses { get; set; }

    

        [ForeignKey("ClassRoom")]
        public int ClassRoomId { get; set; }   
        
        public ClassRoom ClassRoom { get; set; }

        


    }


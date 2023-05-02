
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


    public class Day
    {
        [Key]
        public int DayId { get; set; }
        [Range(1,5)]
        public int DayNum { get; set; }

        [ForeignKey("TimeTable")]
        public int TimeTableId { get; set; }    
        public TimeTable TimeTable { get; set; }

        public List<ClassRoom>? ClassRooms { get; set; } = new List<ClassRoom>();



    }


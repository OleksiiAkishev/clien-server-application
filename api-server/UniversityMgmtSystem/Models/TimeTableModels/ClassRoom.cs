using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


    public class ClassRoom
    {


        [Key]
        public int ClassRoomId { get; set; }
        [Required]
        public string ClassroomName { get; set; }

        [ForeignKey("Day")]
        public int DayId { get; set; }
        public Day? Day { get; set; }

        public List<Slot>? Slots { get; set; } = new List<Slot>();



    }


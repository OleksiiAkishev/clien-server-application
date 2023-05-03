using System.ComponentModel.DataAnnotations;

    public class TimeTable
    {
        [Key]
        public int TimeTableId { get; set; }

        public string TimeTableName { get; set;}

        public List<Day>? Days { get; set; }=new List<Day> ();
    }


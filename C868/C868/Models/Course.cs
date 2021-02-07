using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C868.Models
{
    [Table("courses")]
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int CourseID { get; set; }

        public int TermID { get; set; }

        [MaxLength(50), Unique]
        public string CourseName { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool NotificationsEnabled { get; set; }

        public string Status { get; set; }

        [MaxLength(50)]
        public string InstName { get; set; }

        [MaxLength(20)]
        public string InstPhone { get; set; }

        [MaxLength(50)]
        public string InstEmail { get; set; }

        [MaxLength(250)]
        public string Notes { get; set; }

        [MaxLength(10)]
        public string Grade { get; set; }
    }
}

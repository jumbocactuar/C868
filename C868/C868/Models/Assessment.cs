using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C868.Models
{
    [Table("assessments")]
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int AssessmentID { get; set; }

        public int CourseID { get; set; }

        [MaxLength(50), Unique]
        public string Name { get; set; }

        [MaxLength(10)]
        public string Type { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool NotificationsEnabled { get; set; }
    }
}

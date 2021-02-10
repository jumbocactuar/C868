using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C868.Models
{
    [Table("flashcards")]
    public class FlashCard
    {
        [PrimaryKey, AutoIncrement]
        public int CardID { get; set; }

        public int AssessmentID { get; set; }

        [MaxLength(250)]
        public string Question { get; set; }

        [MaxLength(250)]
        public string Answer { get; set; }

        public string Confidence { get; set; }
    }
}

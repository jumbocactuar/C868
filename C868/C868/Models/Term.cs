using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C868.Models
{
    [Table("terms")]
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int TermID { get; set; }

        [MaxLength(50), Unique]
        public string Title { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}

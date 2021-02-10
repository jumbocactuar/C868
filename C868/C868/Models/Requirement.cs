using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C868.Models
{
    [Table("requirements")]
    public class Requirement
    {
        [PrimaryKey, AutoIncrement]
        public int ReqID { get; set; }

        public int AssessmentID { get; set; }

        [MaxLength(250)]
        public string Req { get; set; }

        [MaxLength(250)]
        public string Notes { get; set; }

        public bool Satisfied { get; set; }
    }
}

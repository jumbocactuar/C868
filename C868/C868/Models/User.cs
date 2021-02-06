using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C868.Models
{
    [Table("users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int UserID { get; set; }

        [MaxLength(20), Unique]
        public string UserName { get; set; }

        [MaxLength(20)]
        public string Password { get; set; }
    }
}

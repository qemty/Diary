using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diary3
{

    [Table("Subjects")]
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("Name")]
        public string Name { get; set; } // Например, "Программирование"

        public Subject() { }
        public Subject(string name)
        {
            Name = name;
        }
    }
}
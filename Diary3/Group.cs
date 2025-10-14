using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary3
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Groups")]
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("Name")]
        public string Name { get; set; } // Например, "Т-192"

        public Group() { }
        public Group(string name)
        {
            Name = name;
        }
    }
}
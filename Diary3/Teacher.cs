using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diary3
{
    [Table("Teachers")]
    public class Teacher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Указываем, что Id не генерируется автоматически
        public int Id { get; set; }

        [Required]
        [Column("Name")]
        public string Name { get; set; }

        [Required]
        [Column("Password")]
        public string Password { get; set; }

        [Required]
        [Column("Role")]
        public Role Role { get; set; }

        public Teacher() { }

        public Teacher(string name, string password, Role role)
        {
            Name = name;
            Password = password;
            Role = role;
        }
    }
}
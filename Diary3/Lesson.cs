using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diary3
{
    [Table("Lessons")]
    public class Lesson
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Column("Place")]
        public string Place { get; set; }

        [Required]
        [Column("DayOfWeek")]
        public int DayOfWeek { get; set; }

        [Required]
        [Column("PairNumber")]
        public int PairNumber { get; set; }

        [Column("WeekNumber")]
        public int WeekNumber { get; set; }

        [Column("IsReplacement")]
        public bool IsReplacement { get; set; }

        [Column("IsRemoved")]
        public bool IsRemoved { get; set; }

        [Column("Notes")]
        public string Notes { get; set; }

        [Column("HoursConducted")]
        public int HoursConducted { get; set; }

        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }


    }
}
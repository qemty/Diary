using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary3
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LessonNotes")]
    public class LessonNote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("DayOfWeek")]
        public int DayOfWeek { get; set; }

        [Required]
        [Column("PairNumber")]
        public int PairNumber { get; set; }

        [Column("Notes")]
        public string Notes { get; set; } // Текст заметки

        [Required]
        public int TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }

        public LessonNote() { }
        public LessonNote(int dayOfWeek, int pairNumber, string notes, int teacherId)
        {
            DayOfWeek = dayOfWeek;
            PairNumber = pairNumber;
            Notes = notes;
            TeacherId = teacherId;
        }
    }
}
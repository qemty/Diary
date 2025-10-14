using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diary3
{
    [Table("LabWorks")]
    public class LabWork
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LessonId { get; set; }

        [Column("Topic")]
        public string Topic { get; set; }

        [Column("Hours")]
        public int Hours { get; set; }

        [Required]
        [Column("Date")]
        public DateTime Date { get; set; }

        [Column("IsCompleted")]
        public bool IsCompleted { get; set; } 

        [ForeignKey("LessonId")]
        public virtual Lesson Lesson { get; set; }
    }
}
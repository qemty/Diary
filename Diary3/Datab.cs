using Diary3;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Diary3
{
    using System.Data.Entity;

    internal class Datab : DbContext
    {
        public Datab() : base("DBConnection")
        {
            // Отключаем ленивую загрузку
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LabWork> LabWorks { get; set; }
        public DbSet<LessonNote> LessonNotes { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace Diary3
{
    public class TeacherSchedule
    {
        public Dictionary<int, Dictionary<int, Lesson>> Lessons { get; set; }

        public TeacherSchedule()
        {
            Lessons = new Dictionary<int, Dictionary<int, Lesson>>();
        }
    }
}

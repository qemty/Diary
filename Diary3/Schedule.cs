using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary3
{
    public class Schedule
    {
        public Dictionary<int, Dictionary<int, Lesson>> Lessons { get; set; } = new Dictionary<int, Dictionary<int, Lesson>>();

        public void AddLesson(int day, int pair, Lesson lesson)
        {
            if (!Lessons.ContainsKey(day))
                Lessons[day] = new Dictionary<int, Lesson>();
            Lessons[day][pair] = lesson;
        }
    }
}
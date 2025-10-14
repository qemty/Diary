using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diary3
{
    public class ScheduleParser
    {
        private readonly HtmlDocument doc;
        private readonly string teacherName;

        public ScheduleParser(HtmlDocument document, string teacherName)
        {
            this.doc = document;
            this.teacherName = teacherName;
        }

        public Schedule GetSchedule()
        {
            var schedule = new Schedule();
            var weeks = new[] { "left_week", "right_week" };
            int weekNumber = 1;

            using (var db = new Datab())
            {
                foreach (var week in weeks)
                {
                    var weekNode = doc.GetElementbyId(week);
                    if (weekNode == null) continue;

                    var table = weekNode.SelectSingleNode(".//table");
                    if (table == null) continue;

                    var rows = table.SelectNodes(".//tr").Skip(2);
                    foreach (var row in rows)
                    {
                        var cells = row.SelectNodes(".//td");
                        if (cells == null || !int.TryParse(cells[0].InnerText, out int pairNumber)) continue;

                        for (int day = 1; day <= 6; day++)
                        {
                            var cell = cells[day];
                            var pairNodes = cell.SelectNodes(".//div[contains(@class, 'pair')]");
                            if (pairNodes == null) continue;

                            foreach (var pairNode in pairNodes)
                            {
                                var subjectNode = pairNode.SelectSingleNode(".//div[@class='subject']/a");
                                var groupNode = pairNode.SelectSingleNode(".//div[@class='group']/span/a");
                                var placeNode = pairNode.SelectSingleNode(".//div[@class='place']/a");

                                string subjectName = subjectNode?.InnerText.Trim() ?? "";
                                string groupName = groupNode?.InnerText.Trim() ?? "";
                                string place = placeNode?.InnerText.Trim() ?? "";

                                if (string.IsNullOrWhiteSpace(subjectName) || string.IsNullOrWhiteSpace(groupName))
                                {
                                    Console.WriteLine($"Skipping lesson due to empty Subject or Group: Day={day}, Pair={pairNumber}");
                                    continue;
                                }

                                // Ищем или создаем предмет
                                var subject = db.Subjects.FirstOrDefault(s => s.Name == subjectName);
                                if (subject == null)
                                {
                                    subject = new Subject { Name = subjectName };
                                    db.Subjects.Add(subject);
                                    db.SaveChanges();
                                }

                                // Ищем или создаем группу
                                var group = db.Groups.FirstOrDefault(g => g.Name == groupName);
                                if (group == null)
                                {
                                    group = new Group { Name = groupName };
                                    db.Groups.Add(group);
                                    db.SaveChanges();
                                }

                                // Проверяем, существует ли урок
                                var lesson = new Lesson
                                {
                                    Subject = subject,
                                    SubjectId = subject.Id,
                                    Group = group,
                                    GroupId = group.Id,
                                    Place = place,
                                    DayOfWeek = day,
                                    PairNumber = pairNumber,
                                    WeekNumber = weekNumber,
                                    IsReplacement = pairNode.GetAttributeValue("class", "").Contains("added"),
                                    IsRemoved = pairNode.GetAttributeValue("class", "").Contains("removed"),
                                    Notes = "",
                                    HoursConducted = 0
                                };

                                schedule.AddLesson(day, pairNumber, lesson);
                            }
                        }
                    }
                    weekNumber++;
                }
            }
            return schedule;
        }
    }
}
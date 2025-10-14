using System;
using System.Collections.Generic;
using System.Data.Entity; // Добавляем для Include (EF6)
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace Diary3
{
    public partial class MainForm : Form
    {
        private Schedule schedule;
        private Dictionary<string, Label> labelDictionary;
        private Button btnReturnToTeacherSchedule;
        private Teacher teacher;
        private bool isTeacherSchedule;
        private Schedule originalSchedule; // Для локальной фильтрации

        // Публичное свойство для доступа к teacher
        public Teacher Teacher => teacher;

        public MainForm(Teacher teacher1)
        {
            this.teacher = teacher1;
            labelDictionary = new Dictionary<string, Label>();
            CreateLabels();
            InitializeComponent();
            this.Text = $"Расписание: {teacher.Name}";
            Console.WriteLine($"MainForm initialized with Teacher: Id={teacher.Id}, Name={teacher.Name}");

            // Исправляем обработчики событий
            lblMonday.Click += new EventHandler((s, e) => OpenDaySchedule(1));
            lblTuesday.Click += new EventHandler((s, e) => OpenDaySchedule(2));
            lblWednesday.Click += new EventHandler((s, e) => OpenDaySchedule(3));
            lblThursday.Click += new EventHandler((s, e) => OpenDaySchedule(4));
            lblFriday.Click += new EventHandler((s, e) => OpenDaySchedule(5));
            lblSaturday.Click += new EventHandler((s, e) => OpenDaySchedule(6));

            btnReturnToTeacherSchedule = new Button
            {
                Text = "Вернуться к расписанию преподавателя",
                Size = new Size(250, 30),
                Location = new Point(10, 10),
                Visible = false
            };
            btnReturnToTeacherSchedule.Click += new EventHandler(BtnReturnToTeacherSchedule_Click);
            this.Controls.Add(btnReturnToTeacherSchedule);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadTeacherSchedule(teacher.Id);
        }

        private void OpenDaySchedule(int day)
        {
            var dayScheduleForm = new DayScheduleForm(day, schedule, teacher, this);
            this.Hide();
            dayScheduleForm.ShowDialog();
        }

        private Schedule LoadScheduleFromDatabase(int teacherId)
        {
            var schedule = new Schedule();
            using (var db = new Datab())
            {
                var lessons = db.Lessons
                    .Include(l => l.Subject)
                    .Include(l => l.Group)
                    .Where(l => l.TeacherId == teacherId)
                    .ToList();

                foreach (var lesson in lessons)
                {
                    if (lesson.Subject == null || lesson.Group == null)
                    {
                        Console.WriteLine($"Lesson Id={lesson.Id} has null Subject or Group. Skipping.");
                        continue;
                    }

                    // Создаем копию объекта Lesson, чтобы он не зависел от контекста
                    var lessonCopy = new Lesson
                    {
                        Id = lesson.Id,
                        TeacherId = lesson.TeacherId,
                        SubjectId = lesson.SubjectId,
                        GroupId = lesson.GroupId,
                        Place = lesson.Place,
                        DayOfWeek = lesson.DayOfWeek,
                        PairNumber = lesson.PairNumber,
                        WeekNumber = lesson.WeekNumber,
                        IsReplacement = lesson.IsReplacement,
                        IsRemoved = lesson.IsRemoved,
                        Notes = lesson.Notes,
                        HoursConducted = lesson.HoursConducted,
                        Subject = new Subject { Id = lesson.Subject.Id, Name = lesson.Subject.Name },
                        Group = new Group { Id = lesson.Group.Id, Name = lesson.Group.Name }
                    };

                    schedule.AddLesson(lesson.DayOfWeek, lesson.PairNumber, lessonCopy);
                }
            }
            Console.WriteLine($"Loaded schedule from database with {schedule.Lessons.Count} days for TeacherId: {teacherId}");
            return schedule;
        }

        private void LoadTeacherSchedule(int teacherId)
        {
            Console.WriteLine($"Loading schedule for TeacherId: {teacherId}, Name: {teacher.Name}");

            // Сначала пытаемся загрузить расписание из базы данных
            schedule = LoadScheduleFromDatabase(teacherId);

            // Если расписание пустое, парсим его с сайта
            if (!schedule.Lessons.Any())
            {
                try
                {
                    var web = new HtmlWeb();
                    var url = $"https://kbp.by/rasp/timetable/view_beta_kbp/?cat=teacher&id={teacherId}";
                    Console.WriteLine($"Parsing schedule from URL: {url}");
                    var doc = web.Load(url);
                    var parser = new ScheduleParser(doc, teacher.Name);
                    schedule = parser.GetSchedule();
                    Console.WriteLine($"Schedule parsed from website with {schedule.Lessons.Count} days for TeacherId: {teacherId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing schedule: {ex.Message}");
                    MessageBox.Show("Ошибка при загрузке расписания с сайта. Проверьте Id преподавателя и доступ к интернету.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Сохраняем расписание в базу данных
                using (var db = new Datab())
                {
                    // Проверяем, существует ли преподаватель
                    var teacherExists = db.Teacher.Any(t => t.Id == teacherId);
                    if (!teacherExists)
                    {
                        db.Teacher.Add(new Teacher
                        {
                            Id = teacherId,
                            Name = teacher.Name,
                            Password = "password123",
                            Role = Role.Teacher
                        });
                        db.SaveChanges();
                        Console.WriteLine($"Added new teacher with Id: {teacherId}, Name: {teacher.Name}");
                    }

                    foreach (var day in schedule.Lessons)
                    {
                        foreach (var lessonEntry in day.Value)
                        {
                            var lesson = lessonEntry.Value;
                            if (lesson == null) continue;

                            var subjectName = lesson.Subject?.Name?.Trim() ?? "";
                            if (string.IsNullOrEmpty(subjectName))
                            {
                                Console.WriteLine("Skipping lesson due to empty Subject name.");
                                continue;
                            }
                            var subject = db.Subjects.FirstOrDefault(s => s.Name == subjectName);
                            if (subject == null)
                            {
                                subject = new Subject { Name = subjectName };
                                db.Subjects.Add(subject);
                                db.SaveChanges();
                                Console.WriteLine($"Added subject: {subject.Name}, Id: {subject.Id}");
                            }
                            lesson.SubjectId = subject.Id;
                            lesson.Subject = subject;

                            var groupName = lesson.Group?.Name?.Trim() ?? "";
                            if (string.IsNullOrEmpty(groupName))
                            {
                                Console.WriteLine("Skipping lesson due to empty Group name.");
                                continue;
                            }
                            var group = db.Groups.FirstOrDefault(g => g.Name == groupName);
                            if (group == null)
                            {
                                group = new Group { Name = groupName };
                                db.Groups.Add(group);
                                db.SaveChanges();
                                Console.WriteLine($"Added group: {group.Name}, Id: {group.Id}");
                            }
                            lesson.GroupId = group.Id;
                            lesson.Group = group;

                            // Проверяем, существует ли урок в базе данных
                            var existingLesson = db.Lessons.FirstOrDefault(l =>
                                l.TeacherId == teacherId &&
                                l.DayOfWeek == lesson.DayOfWeek &&
                                l.PairNumber == lesson.PairNumber &&
                                l.WeekNumber == lesson.WeekNumber);

                            if (existingLesson == null)
                            {
                                var newLesson = new Lesson
                                {
                                    TeacherId = teacherId,
                                    SubjectId = subject.Id,
                                    GroupId = group.Id,
                                    Place = lesson.Place,
                                    DayOfWeek = lesson.DayOfWeek,
                                    PairNumber = lesson.PairNumber,
                                    WeekNumber = lesson.WeekNumber,
                                    IsReplacement = lesson.IsReplacement,
                                    IsRemoved = lesson.IsRemoved,
                                    Notes = lesson.Notes ?? "",
                                    HoursConducted = lesson.HoursConducted
                                };
                                db.Lessons.Add(newLesson);
                                Console.WriteLine($"Adding lesson: TeacherId={newLesson.TeacherId}, SubjectId={newLesson.SubjectId}, GroupId={newLesson.GroupId}, DayOfWeek={newLesson.DayOfWeek}, PairNumber={newLesson.PairNumber}");
                            }
                            else
                            {
                                existingLesson.SubjectId = subject.Id;
                                existingLesson.GroupId = group.Id;
                                existingLesson.Place = lesson.Place;
                                existingLesson.IsReplacement = lesson.IsReplacement;
                                existingLesson.IsRemoved = lesson.IsRemoved;
                                existingLesson.Notes = lesson.Notes ?? "";
                                existingLesson.HoursConducted = lesson.HoursConducted;
                                Console.WriteLine($"Updating lesson: TeacherId={existingLesson.TeacherId}, DayOfWeek={existingLesson.DayOfWeek}, PairNumber={existingLesson.PairNumber}");
                            }
                        }
                    }
                    try
                    {
                        db.SaveChanges();
                        Console.WriteLine("Lessons saved successfully.");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var error in ex.EntityValidationErrors)
                        {
                            Console.WriteLine($"Entity of type {error.Entry.Entity.GetType().Name} in state {error.Entry.State} has the following validation errors:");
                            foreach (var ve in error.ValidationErrors)
                            {
                                Console.WriteLine($"- Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                            }
                        }
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving lessons: {ex.Message}");
                        if (ex.InnerException != null)
                        {
                            Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                        }
                        throw;
                    }
                }

                // После сохранения заново загружаем расписание
                schedule = LoadScheduleFromDatabase(teacherId);
            }

            originalSchedule = schedule; // Сохраняем оригинальное расписание
            isTeacherSchedule = true;
            DisplaySchedule();
        }

        private void LoadSubjectSchedule(int subjectId)
        {
            if (originalSchedule == null || originalSchedule.Lessons == null)
            {
                MessageBox.Show("Оригинальное расписание недоступно для фильтрации.");
                Console.WriteLine("Original schedule is null.");
                return;
            }

            var filteredSchedule = new Schedule
            {
                Lessons = new Dictionary<int, Dictionary<int, Lesson>>()
            };

            foreach (var day in originalSchedule.Lessons)
            {
                var filteredDay = new Dictionary<int, Lesson>();
                foreach (var lessonEntry in day.Value)
                {
                    var lesson = lessonEntry.Value;
                    if (lesson != null && lesson.SubjectId == subjectId)
                    {
                        filteredDay[lessonEntry.Key] = lesson;
                    }
                }
                if (filteredDay.Count > 0)
                {
                    filteredSchedule.Lessons[day.Key] = filteredDay;
                }
            }

            schedule = filteredSchedule;
            Console.WriteLine($"Расписание отфильтровано по предмету {subjectId}. Дней: {schedule.Lessons.Count}");
            isTeacherSchedule = false;
            DisplaySchedule();
        }

        private void LoadGroupSchedule(int groupId)
        {
            if (originalSchedule == null || originalSchedule.Lessons == null)
            {
                MessageBox.Show("Оригинальное расписание недоступно для фильтрации.");
                Console.WriteLine("Original schedule is null.");
                return;
            }

            var filteredSchedule = new Schedule
            {
                Lessons = new Dictionary<int, Dictionary<int, Lesson>>()
            };

            foreach (var day in originalSchedule.Lessons)
            {
                var filteredDay = new Dictionary<int, Lesson>();
                foreach (var lessonEntry in day.Value)
                {
                    var lesson = lessonEntry.Value;
                    if (lesson != null && lesson.GroupId == groupId)
                    {
                        filteredDay[lessonEntry.Key] = lesson;
                    }
                }
                if (filteredDay.Count > 0)
                {
                    filteredSchedule.Lessons[day.Key] = filteredDay;
                }
            }

            schedule = filteredSchedule;
            Console.WriteLine($"Расписание отфильтровано по группе {groupId}. Дней: {schedule.Lessons.Count}");
            isTeacherSchedule = false;
            DisplaySchedule();
        }

        private void LoadPlaceSchedule(string place)
        {
            if (originalSchedule == null || originalSchedule.Lessons == null)
            {
                MessageBox.Show("Original schedule is not available for filtering.");
                Console.WriteLine("Original schedule is null.");
                return;
            }

            var filteredSchedule = new Schedule
            {
                Lessons = new Dictionary<int, Dictionary<int, Lesson>>()
            };

            foreach (var day in originalSchedule.Lessons)
            {
                var filteredDay = new Dictionary<int, Lesson>();
                foreach (var lessonEntry in day.Value)
                {
                    var lesson = lessonEntry.Value;
                    if (lesson != null && lesson.Place != null && lesson.Place.Trim().Equals(place.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        filteredDay[lessonEntry.Key] = lesson;
                    }
                }
                if (filteredDay.Count > 0)
                {
                    filteredSchedule.Lessons[day.Key] = filteredDay;
                }
            }

            schedule = filteredSchedule;
            Console.WriteLine($"Schedule filtered for place {place}. Days: {schedule.Lessons.Count}");
            isTeacherSchedule = false;
            DisplaySchedule();
        }

        private void DisplaySchedule()
        {
            if (schedule == null || schedule.Lessons == null || schedule.Lessons.Count == 0)
            {
                MessageBox.Show("No schedule data available.");
                Console.WriteLine("No schedule data available.");
                return;
            }

            if (labelDictionary == null)
            {
                Console.WriteLine("labelDictionary is null! Reinitializing...");
                labelDictionary = new Dictionary<string, Label>();
                CreateLabels();
            }

            ClearLabels();

            Console.WriteLine($"Displaying schedule. IsTeacherSchedule: {isTeacherSchedule}, Teacher Name: {teacher.Name}");

            for (int day = 1; day <= 6; day++)
            {
                for (int pairNumber = 1; pairNumber <= 13; pairNumber++)
                {
                    Console.WriteLine($"Checking Day={day}, Pair={pairNumber}");
                    bool hasLessonData = schedule.Lessons.ContainsKey(day) &&
                                       schedule.Lessons[day].ContainsKey(pairNumber) &&
                                       schedule.Lessons[day][pairNumber] != null;
                    bool displayTeacherName = hasLessonData &&
                                             !string.IsNullOrEmpty(schedule.Lessons[day][pairNumber].Subject?.Name?.Trim());

                    if (hasLessonData)
                    {
                        var lesson = schedule.Lessons[day][pairNumber];
                        Console.WriteLine($"Lesson exists: {lesson != null}, Subject={lesson?.Subject?.Name}");

                        if (labelDictionary.TryGetValue($"labelSubject{day}_{pairNumber}", out var labelSubject))
                        {
                            labelSubject.Text = lesson.Subject?.Name ?? "_________________";
                            labelSubject.Tag = lesson.SubjectId;
                            labelSubject.Cursor = Cursors.Hand;
                            labelSubject.Click += new EventHandler(LabelSubject_Click);
                            labelSubject.ForeColor = string.IsNullOrEmpty(lesson.Subject?.Name) ? Color.Gray : Color.White;
                        }
                        else
                        {
                            Console.WriteLine($"labelSubject{day}_{pairNumber} not found in labelDictionary");
                        }

                        if (labelDictionary.TryGetValue($"labelTeacher{day}_{pairNumber}", out var labelTeacher))
                        {
                            if (displayTeacherName)
                            {
                                labelTeacher.Text = isTeacherSchedule ? teacher.Name : (lesson.Teacher?.Name ?? "");
                                labelTeacher.ForeColor = Color.White;
                            }
                            else
                            {
                                labelTeacher.Text = "_______";
                                labelTeacher.ForeColor = Color.Gray;
                            }
                            Console.WriteLine($"Setting labelTeacher.Text to: {labelTeacher.Text}");
                        }
                        else
                        {
                            Console.WriteLine($"labelTeacher{day}_{pairNumber} not found in labelDictionary");
                        }

                        if (labelDictionary.TryGetValue($"labelGroup{day}_{pairNumber}", out var labelGroup))
                        {
                            labelGroup.Text = lesson.Group?.Name ?? "____";
                            labelGroup.Tag = lesson.GroupId;
                            labelGroup.Cursor = Cursors.Hand;
                            labelGroup.Click += new EventHandler(LabelGroup_Click);
                            labelGroup.ForeColor = string.IsNullOrEmpty(lesson.Group?.Name) ? Color.Gray : Color.White;
                        }
                        else
                        {
                            Console.WriteLine($"labelGroup{day}_{pairNumber} not found in labelDictionary");
                        }

                        if (labelDictionary.TryGetValue($"labelPlace{day}_{pairNumber}", out var labelPlace))
                        {
                            labelPlace.Text = lesson.Place ?? "___";
                            if (!string.IsNullOrEmpty(lesson.Place))
                            {
                                labelPlace.Tag = lesson.Place;
                                labelPlace.Cursor = Cursors.Hand;
                                labelPlace.Click += new EventHandler(LabelPlace_Click);
                            }
                            labelPlace.ForeColor = string.IsNullOrEmpty(lesson.Place) ? Color.Gray : Color.White;
                        }
                        else
                        {
                            Console.WriteLine($"labelPlace{day}_{pairNumber} not found in labelDictionary");
                        }

                        if (labelDictionary.TryGetValue($"labelPairNumber{day}_{pairNumber}", out var labelPairNumber))
                        {
                            labelPairNumber.Text = pairNumber.ToString();
                            labelPairNumber.ForeColor = Color.White;
                        }
                        else
                        {
                            Console.WriteLine($"labelPairNumber{day}_{pairNumber} not found in labelDictionary");
                        }
                    }
                    else
                    {
                        SetPlaceholderText(day, pairNumber);
                    }

                    DrawLessonBorder(day, pairNumber);
                }
            }

            btnReturnToTeacherSchedule.Visible = !isTeacherSchedule;
        }

        private void ClearLabels()
        {
            if (labelDictionary == null)
            {
                Console.WriteLine("labelDictionary is null in ClearLabels!");
                return;
            }

            foreach (var label in labelDictionary.Values)
            {
                label.Text = string.Empty;
                label.Tag = null;
                label.Cursor = Cursors.Default;
                label.Click -= LabelSubject_Click;
                label.Click -= LabelGroup_Click;
                label.Click -= LabelPlace_Click;
                label.ForeColor = Color.Gray;
            }
        }

        private void CreateLabels()
        {
            int labelWidth = 90;
            int labelHeight = 20;
            int startX = 80;
            int startY = 80;
            int spacingX = 240;
            int spacingY = 55;

            for (int day = 1; day <= 6; day++)
            {
                for (int lesson = 1; lesson <= 13; lesson++)
                {
                    int x = startX + (day - 1) * spacingX;
                    int y = startY + (lesson - 1) * spacingY;

                    CreateLabel($"labelPairNumber{day}_{lesson}", x - 30, y, 30, labelHeight);
                    CreateLabel($"labelSubject{day}_{lesson}", x, y, labelWidth, labelHeight);
                    CreateLabel($"labelGroup{day}_{lesson}", x + labelWidth, y, labelWidth, labelHeight);
                    CreateLabel($"labelTeacher{day}_{lesson}", x, y + labelHeight, labelWidth, labelHeight);
                    CreateLabel($"labelPlace{day}_{lesson}", x + labelWidth, y + labelHeight, labelWidth, labelHeight);
                }
            }

            Console.WriteLine("Labels created.");
        }

        private void CreateLabel(string name, int x, int y, int width, int height)
        {
            var label = new Label
            {
                Name = name,
                Size = new Size(width, height),
                Location = new Point(x, y),
                Text = "",
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(label);
            labelDictionary[name] = label;
        }

        private void SetPlaceholderText(int day, int pairNumber)
        {
            if (labelDictionary.TryGetValue($"labelSubject{day}_{pairNumber}", out var labelSubject))
            {
                labelSubject.Text = "_________________";
                labelSubject.ForeColor = Color.Gray;
            }

            if (labelDictionary.TryGetValue($"labelTeacher{day}_{pairNumber}", out var labelTeacher))
            {
                labelTeacher.Text = "_______";
                labelTeacher.ForeColor = Color.Gray;
            }

            if (labelDictionary.TryGetValue($"labelGroup{day}_{pairNumber}", out var labelGroup))
            {
                labelGroup.Text = "____";
                labelGroup.ForeColor = Color.Gray;
            }

            if (labelDictionary.TryGetValue($"labelPlace{day}_{pairNumber}", out var labelPlace))
            {
                labelPlace.Text = "___";
                labelPlace.ForeColor = Color.Gray;
            }

            if (labelDictionary.TryGetValue($"labelPairNumber{day}_{pairNumber}", out var labelPairNumber))
            {
                labelPairNumber.Text = pairNumber.ToString();
                labelPairNumber.ForeColor = Color.Gray;
            }
        }

        private void DrawLessonBorder(int day, int pairNumber)
        {
            if (labelDictionary.TryGetValue($"labelSubject{day}_{pairNumber}", out var labelSubject) &&
                labelDictionary.TryGetValue($"labelTeacher{day}_{pairNumber}", out var labelTeacher) &&
                labelDictionary.TryGetValue($"labelGroup{day}_{pairNumber}", out var labelGroup) &&
                labelDictionary.TryGetValue($"labelPlace{day}_{pairNumber}", out var labelPlace))
            {
                labelSubject.BorderStyle = BorderStyle.FixedSingle;
                labelTeacher.BorderStyle = BorderStyle.FixedSingle;
                labelGroup.BorderStyle = BorderStyle.FixedSingle;
                labelPlace.BorderStyle = BorderStyle.FixedSingle;

                int spacing = 22;

                labelSubject.Location = new Point(labelSubject.Location.X, labelSubject.Location.Y);
                labelGroup.Location = new Point(labelSubject.Location.X + labelSubject.Width, labelSubject.Location.Y);
                labelTeacher.Location = new Point(labelSubject.Location.X, labelSubject.Location.Y + spacing);
                labelPlace.Location = new Point(labelSubject.Location.X + labelSubject.Width, labelSubject.Location.Y + spacing);

                labelSubject.Size = new Size(labelSubject.Width, spacing);
                labelGroup.Size = new Size(60, spacing);
                labelGroup.TextAlign = ContentAlignment.MiddleRight;
                labelTeacher.Size = new Size(labelTeacher.Width, spacing);
                labelPlace.Size = new Size(60, spacing);
                labelPlace.TextAlign = ContentAlignment.MiddleRight;
            }
        }

        private void LabelSubject_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is Label label && label.Tag != null && int.TryParse(label.Tag.ToString(), out int subjectId))
                {
                    LoadSubjectSchedule(subjectId);
                }
                else
                {
                    MessageBox.Show("Label or Tag is null or Tag is not an integer.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
            }
        }

        private void LabelGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is Label label && label.Tag != null && int.TryParse(label.Tag.ToString(), out int groupId))
                {
                    LoadGroupSchedule(groupId);
                }
                else
                {
                    MessageBox.Show("Label or Tag is null or Tag is not an integer.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
            }
        }

        private void LabelPlace_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is Label label && label.Tag != null)
                {
                    string place = label.Tag.ToString();
                    LoadPlaceSchedule(place);
                }
                else
                {
                    MessageBox.Show("Label or Tag is null.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
            }
        }

        private void BtnReturnToTeacherSchedule_Click(object sender, EventArgs e)
        {
            LoadTeacherSchedule(teacher.Id);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var calculateHoursForm = new CalculateHoursForm(this);
            this.Hide();
            calculateHoursForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Пожалуйста, введите целое число!", "Некорректный ввод!");
        }
    }
}
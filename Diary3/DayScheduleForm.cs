using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ClosedXML.Excel;
using System.IO;

namespace Diary3
{
    public partial class DayScheduleForm : Form
    {
        private int currentDay;
        private Schedule schedule;
        private Teacher teacher;
        private MainForm mainForm;

        public DayScheduleForm(int day, Schedule schedule, Teacher teacher, MainForm mainForm)
        {
            this.currentDay = day;
            this.schedule = schedule ?? new Schedule();
            this.teacher = teacher ?? throw new ArgumentNullException(nameof(teacher));
            this.mainForm = mainForm ?? throw new ArgumentNullException(nameof(mainForm));
            InitializeComponent();
            dgvSchedule.CellValidating += DgvSchedule_CellValidating;
            LoadDaySchedule();
        }

        private void LoadDaySchedule()
        {
            this.Text = $"Расписание на {GetDayName(currentDay)}";
            lblDay.Text = GetDayName(currentDay);

            if (schedule == null || !schedule.Lessons.ContainsKey(currentDay))
            {
                MessageBox.Show("Нет данных о расписании на этот день.");
                dgvSchedule.Rows.Clear();
                lblTotalHours.Text = "Всего часов за день: 0";
                lblWeeklyHours.Text = "Всего часов за неделю: 0";
                return;
            }

            var lessons = schedule.Lessons[currentDay];
            dgvSchedule.Rows.Clear();
            int totalHours = 0;

            using (var db = new Datab())
            {
                db.Database.Log = Console.WriteLine;

                var today = DateTime.Today;

                foreach (var lessonEntry in lessons)
                {
                    int pairNumber = lessonEntry.Key;
                    var lesson = lessonEntry.Value;
                    if (lesson == null) continue;

                    if (lesson.SubjectId == 0 || lesson.GroupId == 0)
                    {
                        Console.WriteLine($"Skipping lesson due to invalid SubjectId ({lesson.SubjectId}) or GroupId ({lesson.GroupId})");
                        continue;
                    }

                    var dbLesson = db.Lessons.FirstOrDefault(l => l.Id == lesson.Id);
                    if (dbLesson == null)
                    {
                        dbLesson = new Lesson
                        {
                            TeacherId = teacher.Id,
                            SubjectId = lesson.SubjectId,
                            GroupId = lesson.GroupId,
                            Place = lesson.Place,
                            DayOfWeek = currentDay,
                            PairNumber = pairNumber,
                            WeekNumber = lesson.WeekNumber,
                            IsReplacement = lesson.IsReplacement,
                            IsRemoved = lesson.IsRemoved,
                            Notes = lesson.Notes,
                            HoursConducted = lesson.HoursConducted
                        };
                        db.Lessons.Add(dbLesson);
                        db.SaveChanges();
                        lesson.Id = dbLesson.Id;
                    }
                    else
                    {
                        lesson.Notes = dbLesson.Notes;
                        lesson.HoursConducted = dbLesson.HoursConducted;
                    }

                    var labWorks = db.LabWorks
                        .Where(lw => lw.LessonId == lesson.Id && lw.Date != null)
                        .AsEnumerable()
                        .Where(lw => lw.Date.Date == today)
                        .ToList();

                    var labWork = labWorks.FirstOrDefault();

                    string topic = labWork?.Topic ?? "";
                    int hours = labWork?.Hours ?? lesson.HoursConducted;
                    bool isCompleted = labWork?.IsCompleted ?? false;

                    totalHours += hours;

                    dgvSchedule.Rows.Add(pairNumber, lesson.Subject?.Name, lesson.Group?.Name,
                        lesson.Place, lesson.Notes, topic, hours, isCompleted);
                }
            }

            lblTotalHours.Text = $"Всего часов за день: {totalHours}";
            lblWeeklyHours.Text = $"Всего часов за неделю: {CalculateWeeklyHours()}";
        }

        private int CalculateWeeklyHours()
        {
            int totalHours = 0;
            using (var db = new Datab())
            {
                db.Database.Log = Console.WriteLine;

                // Определяем текущую дату и начало/конец недели
                var today = DateTime.Today;
                var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
                if (today.DayOfWeek == DayOfWeek.Sunday)
                {
                    startOfWeek = startOfWeek.AddDays(-7);
                }
                var endOfWeek = startOfWeek.AddDays(6);

                // Определяем WeekNumber на основе текущего дня
                int weekNumber = 0;
                if (schedule.Lessons.ContainsKey(currentDay) && schedule.Lessons[currentDay].Any())
                {
                    var firstLesson = schedule.Lessons[currentDay].First().Value;
                    if (firstLesson != null)
                    {
                        weekNumber = firstLesson.WeekNumber;
                    }
                }

                Console.WriteLine($"Calculating weekly hours for TeacherId: {teacher.Id}, WeekNumber: {weekNumber}");
                Console.WriteLine($"Date range: {startOfWeek:dd.MM.yyyy} - {endOfWeek:dd.MM.yyyy}");

                // Получаем дни, которые есть в расписании (понедельник-суббота)
                var scheduledDays = schedule.Lessons
                    .Where(day => day.Key >= 1 && day.Key <= 6)
                    .Select(day => day.Key)
                    .ToList();

                // Загружаем LabWorks с фильтрацией
                var labWorksQuery = db.LabWorks
                    .Include(lw => lw.Lesson)
                    .Where(lw => lw.Lesson.TeacherId == teacher.Id && lw.Date != null);

                // Фильтруем по WeekNumber, если он определен
                if (weekNumber > 0)
                {
                    labWorksQuery = labWorksQuery.Where(lw => lw.Lesson.WeekNumber == weekNumber);
                }

                // Фильтруем по диапазону дат и дням недели
                var labWorks = labWorksQuery
                    .AsEnumerable()
                    .Where(lw => lw.Date.Date >= startOfWeek && lw.Date.Date <= endOfWeek)
                    .Where(lw => scheduledDays.Contains(GetDayOfWeekFromDate(lw.Date)))
                    .ToList();

                Console.WriteLine($"Found {labWorks.Count} LabWorks entries for the week (filtered by scheduled days).");
                foreach (var lw in labWorks)
                {
                    Console.WriteLine($"LabWork: LessonId={lw.LessonId}, Date={lw.Date:dd.MM.yyyy}, Hours={lw.Hours}, WeekNumber={lw.Lesson.WeekNumber}, DayOfWeek={GetDayOfWeekFromDate(lw.Date)}");
                    totalHours += lw.Hours;
                }
            }
            return totalHours;
        }

        private int GetDayOfWeekFromDate(DateTime date)
        {
            int dayOfWeek = (int)date.DayOfWeek;
            if (dayOfWeek == 0) return 7;
            return dayOfWeek;
        }

        private string GetDayName(int day)
        {
            switch (day)
            {
                case 1: return "Понедельник";
                case 2: return "Вторник";
                case 3: return "Среда";
                case 4: return "Четверг";
                case 5: return "Пятница";
                case 6: return "Суббота";
                default: return "Неизвестный день";
            }
        }

        private void DgvSchedule_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvSchedule.Columns[e.ColumnIndex].Name == "Hours")
            {
                if (!int.TryParse(e.FormattedValue?.ToString(), out int hours) || hours < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Пожалуйста, введите положительное целое число в поле 'Часы'.", "Ошибка ввода",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void BtnSaveNotes_Click(object sender, EventArgs e)
        {
            using (var db = new Datab())
            {
                foreach (DataGridViewRow row in dgvSchedule.Rows)
                {
                    if (row.Cells["PairNumber"].Value == null) continue;

                    int pairNumber = Convert.ToInt32(row.Cells["PairNumber"].Value);
                    string notes = row.Cells["Notes"].Value?.ToString();
                    string topic = row.Cells["Topic"].Value?.ToString();
                    int hours = Convert.ToInt32(row.Cells["Hours"].Value ?? 0);
                    bool isCompleted = Convert.ToBoolean(row.Cells["IsCompleted"].Value);

                    var lesson = schedule.Lessons[currentDay][pairNumber];
                    lesson.Notes = notes;
                    lesson.HoursConducted = hours;

                    var dbLesson = db.Lessons.FirstOrDefault(l => l.Id == lesson.Id);
                    if (dbLesson == null)
                    {
                        dbLesson = new Lesson
                        {
                            TeacherId = teacher.Id,
                            SubjectId = lesson.SubjectId,
                            GroupId = lesson.GroupId,
                            Place = lesson.Place,
                            DayOfWeek = currentDay,
                            PairNumber = pairNumber,
                            WeekNumber = lesson.WeekNumber,
                            IsReplacement = lesson.IsReplacement,
                            IsRemoved = lesson.IsRemoved,
                            Notes = notes,
                            HoursConducted = hours
                        };
                        db.Lessons.Add(dbLesson);
                        db.SaveChanges();
                        lesson.Id = dbLesson.Id;
                    }
                    else
                    {
                        dbLesson.Notes = notes;
                        dbLesson.HoursConducted = hours;
                    }

                    var today = DateTime.Today;
                    var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
                    if (today.DayOfWeek == DayOfWeek.Sunday)
                    {
                        startOfWeek = startOfWeek.AddDays(-7);
                    }
                    var lessonDate = startOfWeek.AddDays(currentDay - 1);

                    var labWork = db.LabWorks
                        .AsEnumerable()
                        .FirstOrDefault(lw => lw.LessonId == lesson.Id &&
                            lw.Date != null && lw.Date.Date == lessonDate);

                    if (labWork != null)
                    {
                        labWork.Topic = topic;
                        labWork.Hours = hours;
                        labWork.IsCompleted = isCompleted;
                        labWork.Date = lessonDate;
                    }
                    else if (!string.IsNullOrEmpty(topic) || hours > 0 || isCompleted)
                    {
                        db.LabWorks.Add(new LabWork
                        {
                            LessonId = lesson.Id,
                            Topic = topic,
                            Hours = hours,
                            Date = lessonDate,
                            IsCompleted = isCompleted
                        });
                    }
                }
                db.SaveChanges();
            }
            MessageBox.Show("Данные успешно сохранены.");
            LoadDaySchedule();
        }

        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            using (var reportForm = new ReportForm(mainForm, teacher))
            {
                if (reportForm.ShowDialog() == DialogResult.OK)
                {
                    GenerateReport(reportForm.StartDate, reportForm.EndDate, reportForm.ReportType, reportForm.ExportFormat);
                }
            }
        }

        private void GenerateReport(DateTime startDate, DateTime endDate, string reportType, string exportFormat)
        {
            if (reportType == "По предметам и группам")
            {
                var reportData = GenerateSubjectGroupReport(startDate, endDate);
                if (reportData.Length == 0)
                {
                    MessageBox.Show("Нет данных для формирования отчета.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (exportFormat == "Word")
                    ExportToWordSubjectGroupReport(reportData, startDate, endDate);
                else
                    ExportToExcelSubjectGroupReport(reportData, startDate, endDate);
            }
            else if (reportType == "По загруженности преподавателя")
            {
                var reportData = GenerateWorkloadReport(startDate, endDate);
                if (reportData.Length == 0)
                {
                    MessageBox.Show("Нет данных для формирования отчета.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (exportFormat == "Word")
                    ExportToWordWorkloadReport(reportData, startDate, endDate);
                else
                    ExportToExcelWorkloadReport(reportData, startDate, endDate);
            }
        }

        private (string SubjectName, string GroupName, int Hours)[] GenerateSubjectGroupReport(DateTime startDate, DateTime endDate)
        {
            using (var db = new Datab())
            {
                int weekNumber = 0;
                if (schedule.Lessons.Any())
                {
                    var firstLesson = schedule.Lessons.First().Value.First().Value;
                    if (firstLesson != null)
                    {
                        weekNumber = firstLesson.WeekNumber;
                    }
                }

                var labWorksQuery = db.LabWorks
                    .Include(lw => lw.Lesson)
                    .Include(lw => lw.Lesson.Subject)
                    .Include(lw => lw.Lesson.Group)
                    .Where(lw => lw.Lesson.TeacherId == teacher.Id && lw.Date >= startDate && lw.Date <= endDate);

                if (weekNumber > 0)
                {
                    labWorksQuery = labWorksQuery.Where(lw => lw.Lesson.WeekNumber == weekNumber);
                }

                var labWorks = labWorksQuery
                    .AsEnumerable()
                    .Where(lw => GetDayOfWeekFromDate(lw.Date) >= 1 && GetDayOfWeekFromDate(lw.Date) <= 6)
                    .ToList();

                var reportData = labWorks
                    .GroupBy(lw => new { SubjectName = lw.Lesson.Subject.Name, GroupName = lw.Lesson.Group.Name })
                    .Select(g => new
                    {
                        SubjectName = g.Key.SubjectName,
                        GroupName = g.Key.GroupName,
                        Hours = g.Sum(lw => lw.Hours)
                    })
                    .OrderBy(r => r.SubjectName)
                    .ThenBy(r => r.GroupName)
                    .ToList();

                return reportData.Select(r => (r.SubjectName, r.GroupName, r.Hours)).ToArray();
            }
        }

        private (DateTime Date, int Hours)[] GenerateWorkloadReport(DateTime startDate, DateTime endDate)
        {
            using (var db = new Datab())
            {
                int weekNumber = 0;
                if (schedule.Lessons.Any())
                {
                    var firstLesson = schedule.Lessons.First().Value.First().Value;
                    if (firstLesson != null)
                    {
                        weekNumber = firstLesson.WeekNumber;
                    }
                }

                var labWorksQuery = db.LabWorks
                    .Include(lw => lw.Lesson)
                    .Where(lw => lw.Lesson.TeacherId == teacher.Id && lw.Date >= startDate && lw.Date <= endDate);

                if (weekNumber > 0)
                {
                    labWorksQuery = labWorksQuery.Where(lw => lw.Lesson.WeekNumber == weekNumber);
                }

                var labWorks = labWorksQuery
                    .AsEnumerable()
                    .Where(lw => GetDayOfWeekFromDate(lw.Date) >= 1 && GetDayOfWeekFromDate(lw.Date) <= 6)
                    .ToList();

                var reportData = labWorks
                    .GroupBy(lw => lw.Date.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Hours = g.Sum(lw => lw.Hours)
                    })
                    .OrderBy(r => r.Date)
                    .ToList();

                return reportData.Select(r => (r.Date, r.Hours)).ToArray();
            }
        }

        private void ExportToWordSubjectGroupReport((string SubjectName, string GroupName, int Hours)[] reportData, DateTime startDate, DateTime endDate)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Word Document (*.docx)|*.docx";
                saveFileDialog.Title = "Сохранить отчет по предметам и группам";
                saveFileDialog.FileName = $"SubjectGroupReport_{DateTime.Now:yyyyMMdd_HHmmss}.docx";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Сохранение отчета отменено.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string filePath = saveFileDialog.FileName;
                try
                {
                    using (WordprocessingDocument doc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart mainPart = doc.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        Body body = mainPart.Document.AppendChild(new Body());

                        Paragraph title = body.AppendChild(new Paragraph());
                        Run titleRun = title.AppendChild(new Run());
                        titleRun.AppendChild(new Text($"Отчет по предметам и группам за период {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}"));
                        titleRun.RunProperties = new RunProperties(new Bold());

                        Table table = body.AppendChild(new Table());
                        TableProperties tableProps = new TableProperties(
                            new TableBorders(
                                new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 }
                            )
                        );
                        table.AppendChild(tableProps);

                        TableRow headerRow = table.AppendChild(new TableRow());
                        headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text("Предмет")))));
                        headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text("Группа")))));
                        headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text("Часы")))));

                        foreach (var row in reportData)
                        {
                            TableRow dataRow = table.AppendChild(new TableRow());
                            dataRow.AppendChild(new TableCell(new Paragraph(new Run(new Text(row.SubjectName)))));
                            dataRow.AppendChild(new TableCell(new Paragraph(new Run(new Text(row.GroupName)))));
                            dataRow.AppendChild(new TableCell(new Paragraph(new Run(new Text(row.Hours.ToString())))));
                        }
                    }
                    OpenFileLocation(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении отчета: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Аналогично обновляем остальные методы экспорта
        private void ExportToExcelSubjectGroupReport((string SubjectName, string GroupName, int Hours)[] reportData, DateTime startDate, DateTime endDate)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Сохранить отчет по предметам и группам";
                saveFileDialog.FileName = $"SubjectGroupReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Сохранение отчета отменено.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string filePath = saveFileDialog.FileName;
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Отчет");
                        worksheet.Cell(1, 1).Value = $"Отчет по предметам и группам за период {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}";
                        worksheet.Range(1, 1, 1, 3).Merge().Style.Font.Bold = true;

                        worksheet.Cell(2, 1).Value = "Предмет";
                        worksheet.Cell(2, 2).Value = "Группа";
                        worksheet.Cell(2, 3).Value = "Часы";
                        worksheet.Range(2, 1, 2, 3).Style.Font.Bold = true;

                        for (int i = 0; i < reportData.Length; i++)
                        {
                            worksheet.Cell(i + 3, 1).Value = reportData[i].SubjectName;
                            worksheet.Cell(i + 3, 2).Value = reportData[i].GroupName;
                            worksheet.Cell(i + 3, 3).Value = reportData[i].Hours;
                        }

                        worksheet.Columns().AdjustToContents();
                        workbook.SaveAs(filePath);
                    }
                    OpenFileLocation(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении отчета: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportToWordWorkloadReport((DateTime Date, int Hours)[] reportData, DateTime startDate, DateTime endDate)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Word Document (*.docx)|*.docx";
                saveFileDialog.Title = "Сохранить отчет по загруженности преподавателя";
                saveFileDialog.FileName = $"WorkloadReport_{DateTime.Now:yyyyMMdd_HHmmss}.docx";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Сохранение отчета отменено.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string filePath = saveFileDialog.FileName;
                try
                {
                    using (WordprocessingDocument doc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart mainPart = doc.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        Body body = mainPart.Document.AppendChild(new Body());

                        Paragraph title = body.AppendChild(new Paragraph());
                        Run titleRun = title.AppendChild(new Run());
                        titleRun.AppendChild(new Text($"Отчет по загруженности преподавателя за период {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}"));
                        titleRun.RunProperties = new RunProperties(new Bold());

                        Table table = body.AppendChild(new Table());
                        TableProperties tableProps = new TableProperties(
                            new TableBorders(
                                new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                                new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 }
                            )
                        );
                        table.AppendChild(tableProps);

                        TableRow headerRow = table.AppendChild(new TableRow());
                        headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text("Дата")))));
                        headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text("Часы")))));

                        foreach (var row in reportData)
                        {
                            TableRow dataRow = table.AppendChild(new TableRow());
                            dataRow.AppendChild(new TableCell(new Paragraph(new Run(new Text(row.Date.ToString("dd.MM.yyyy"))))));
                            dataRow.AppendChild(new TableCell(new Paragraph(new Run(new Text(row.Hours.ToString())))));
                        }
                    }
                    OpenFileLocation(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении отчета: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportToExcelWorkloadReport((DateTime Date, int Hours)[] reportData, DateTime startDate, DateTime endDate)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Сохранить отчет по загруженности преподавателя";
                saveFileDialog.FileName = $"WorkloadReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Сохранение отчета отменено.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string filePath = saveFileDialog.FileName;
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Отчет");
                        worksheet.Cell(1, 1).Value = $"Отчет по загруженности преподавателя за период {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}";
                        worksheet.Range(1, 1, 1, 2).Merge().Style.Font.Bold = true;

                        worksheet.Cell(2, 1).Value = "Дата";
                        worksheet.Cell(2, 2).Value = "Часы";
                        worksheet.Range(2, 1, 2, 2).Style.Font.Bold = true;

                        for (int i = 0; i < reportData.Length; i++)
                        {
                            worksheet.Cell(i + 3, 1).Value = reportData[i].Date.ToString("dd.MM.yyyy");
                            worksheet.Cell(i + 3, 2).Value = reportData[i].Hours;
                        }

                        worksheet.Columns().AdjustToContents();
                        workbook.SaveAs(filePath);
                    }
                    OpenFileLocation(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении отчета: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OpenFileLocation(string filePath)
        {
            if (MessageBox.Show("Отчет успешно сохранен. Открыть папку с файлом?", "Успех", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                try
                {
                    string folderPath = Path.GetDirectoryName(filePath);
                    System.Diagnostics.Process.Start("explorer.exe", folderPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось открыть папку: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DayScheduleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.Show();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPrevDay_Click(object sender, EventArgs e)
        {
            if (currentDay > 1)
            {
                currentDay--;
                LoadDaySchedule();
            }
        }

        private void BtnNextDay_Click(object sender, EventArgs e)
        {
            if (currentDay < 6)
            {
                currentDay++;
                LoadDaySchedule();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Diary3
{
    public partial class CalculateHoursForm : Form
    {
        private readonly MainForm mainForm;

        public CalculateHoursForm(MainForm mainForm)
        {
            this.mainForm = mainForm ?? throw new ArgumentNullException(nameof(mainForm));
            InitializeComponent();
            dtpStartDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpEndDate.Value = DateTime.Today;
            LoadScheduleData();
        }

        private void LoadScheduleData()
        {
            var teacher = mainForm.Teacher;
            if (teacher == null)
            {
                MessageBox.Show("Не удалось загрузить данные преподавателя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var db = new Datab())
            {
                var lessons = db.Lessons
                    .Where(l => l.TeacherId == teacher.Id && l.HoursConducted > 0)
                    .ToList();

                if (!lessons.Any())
                {
                    MessageBox.Show("Нет данных о проведенных часах для этого преподавателя.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Очищаем DataGridView перед загрузкой
                foreach (DataGridViewRow row in dgvHours1.Rows)
                {
                    row.Cells["Date"].Value = "";
                    row.Cells["Hours"].Value = "0";
                }
                foreach (DataGridViewRow row in dgvHours2.Rows)
                {
                    row.Cells["Date"].Value = "";
                    row.Cells["Hours"].Value = "0";
                }

                // Вычисляем даты уроков
                var lessonData = new List<(DateTime Date, int Hours)>();
                int selectedYear = int.Parse(cbYear.SelectedItem.ToString());
                DateTime referenceDate = new DateTime(selectedYear, DateTime.Today.Month, 1); // Начало текущего месяца
                while (referenceDate.DayOfWeek != DayOfWeek.Monday)
                {
                    referenceDate = referenceDate.AddDays(1); // Находим первый понедельник месяца
                }

                foreach (var lesson in lessons)
                {
                    int weekOffset = (lesson.WeekNumber - 1) * 7;
                    int dayOffset = lesson.DayOfWeek - 1;
                    DateTime lessonDate = referenceDate.AddDays(weekOffset + dayOffset);

                    // Проверяем, что дата находится в выбранном году
                    if (lessonDate.Year == selectedYear)
                    {
                        lessonData.Add((lessonDate, lesson.HoursConducted));
                    }
                }

                if (!lessonData.Any())
                {
                    MessageBox.Show($"Нет данных о проведенных часах в {selectedYear} году.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                lessonData = lessonData.OrderBy(ld => ld.Date).ToList();

                int rowIndex1 = 0;
                int rowIndex2 = 0;

                foreach (var (date, hours) in lessonData)
                {
                    if (rowIndex1 < dgvHours1.Rows.Count)
                    {
                        dgvHours1.Rows[rowIndex1].Cells["Date"].Value = date.ToString("yyyy-MM-dd");
                        dgvHours1.Rows[rowIndex1].Cells["Hours"].Value = hours.ToString();
                        rowIndex1++;
                    }
                    else if (rowIndex2 < dgvHours2.Rows.Count)
                    {
                        dgvHours2.Rows[rowIndex2].Cells["Date"].Value = date.ToString("yyyy-MM-dd");
                        dgvHours2.Rows[rowIndex2].Cells["Hours"].Value = hours.ToString();
                        rowIndex2++;
                    }
                }
            }
        }

        private void CbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadScheduleData(); // Перезагружаем данные при изменении года
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            DateTime startDate = dtpStartDate.Value.Date;
            DateTime endDate = dtpEndDate.Value.Date;

            if (startDate > endDate)
            {
                MessageBox.Show("Дата начала не может быть позже даты окончания!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int totalHours = 0;
            totalHours += CalculateHoursInGrid(dgvHours1, startDate, endDate);
            totalHours += CalculateHoursInGrid(dgvHours2, startDate, endDate);

            lblTotalHours.Text = $"Общее количество часов: {totalHours}";
        }

        private int CalculateHoursInGrid(DataGridView grid, DateTime startDate, DateTime endDate)
        {
            int hours = 0;

            foreach (DataGridViewRow row in grid.Rows)
            {
                string dateStr = row.Cells["Date"].Value?.ToString();
                if (string.IsNullOrWhiteSpace(dateStr))
                    continue;

                if (!DateTime.TryParse(dateStr, out DateTime rowDate))
                {
                    MessageBox.Show($"Некорректный формат даты в строке {row.Cells["Number"].Value}: {dateStr}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                if (rowDate.Date >= startDate && rowDate.Date <= endDate)
                {
                    string hoursStr = row.Cells["Hours"].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(hoursStr))
                        continue;

                    if (!int.TryParse(hoursStr, out int rowHours) || rowHours < 0)
                    {
                        MessageBox.Show($"Некорректное количество часов в строке {row.Cells["Number"].Value}: {hoursStr}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }

                    hours += rowHours;
                }
            }

            return hours;
        }

        private void CalculateHoursForm_FormClosed(object sender, FormClosingEventArgs e)
        {
            mainForm.Show();
        }
    }
}
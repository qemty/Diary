using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Diary3
{
    public partial class ReportForm : Form
    {
        private MainForm mainForm;
        private Teacher teacher;

        public DateTime StartDate => dtpStartDate.Value;
        public DateTime EndDate => dtpEndDate.Value;
        public string ReportType => cmbReportType.SelectedItem?.ToString();
        public string ExportFormat => cmbExportFormat.SelectedItem?.ToString();

        public ReportForm(MainForm mainForm, Teacher teacher)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.teacher = teacher;

            // Инициализация элементов управления
            dtpStartDate.Value = DateTime.Today.AddDays(-7); // По умолчанию: неделя назад
            dtpEndDate.Value = DateTime.Today;

            cmbReportType.Items.AddRange(new[] { "По предметам и группам", "По загруженности преподавателя" });
            cmbReportType.SelectedIndex = 0;

            cmbExportFormat.Items.AddRange(new[] { "Word", "Excel" });
            cmbExportFormat.SelectedIndex = 0;
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (StartDate > EndDate)
            {
                MessageBox.Show("Дата начала не может быть позже даты окончания.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.Show();
        }
    }
}
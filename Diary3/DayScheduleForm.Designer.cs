using System.Windows.Forms;
using System;
using System.Drawing;

namespace Diary3
{
    partial class DayScheduleForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvSchedule;
        private Label lblDay;
        private Label lblTotalHours;
        private Label lblWeeklyHours;
        private Button btnSaveNotes;
        private Button btnBack;
        private Button btnPrevDay;
        private Button btnNextDay;
        private Button btnGenerateReport;

        private void InitializeComponent()
        {
            this.dgvSchedule = new DataGridView();
            this.lblDay = new Label();
            this.lblTotalHours = new Label();
            this.lblWeeklyHours = new Label();
            this.btnSaveNotes = new Button();
            this.btnBack = new Button();
            this.btnPrevDay = new Button();
            this.btnNextDay = new Button();
            this.btnGenerateReport = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedule)).BeginInit();
            this.SuspendLayout();

            // dgvSchedule
            this.dgvSchedule.BackgroundColor = Color.FromArgb(45, 45, 45);
            this.dgvSchedule.ForeColor = Color.White;
            this.dgvSchedule.GridColor = Color.Gray;
            this.dgvSchedule.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
            this.dgvSchedule.DefaultCellStyle.ForeColor = Color.White;
            this.dgvSchedule.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            this.dgvSchedule.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvSchedule.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.dgvSchedule.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            this.dgvSchedule.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvSchedule.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSchedule.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "PairNumber", HeaderText = "Пара", ReadOnly = true, Width = 50 },
                new DataGridViewTextBoxColumn { Name = "Subject", HeaderText = "Предмет", ReadOnly = true, Width = 150 },
                new DataGridViewTextBoxColumn { Name = "Group", HeaderText = "Группа", ReadOnly = true, Width = 100 },
                new DataGridViewTextBoxColumn { Name = "Place", HeaderText = "Место", ReadOnly = true, Width = 100 },
                new DataGridViewTextBoxColumn { Name = "Notes", HeaderText = "Примечания", Width = 150 },
                new DataGridViewTextBoxColumn { Name = "Topic", HeaderText = "Тема", Width = 150 },
                new DataGridViewTextBoxColumn { Name = "Hours", HeaderText = "Часы", Width = 50 },
                new DataGridViewCheckBoxColumn { Name = "IsCompleted", HeaderText = "Выполнено", Width = 80 }
            });
            this.dgvSchedule.Location = new Point(10, 40);
            this.dgvSchedule.Size = new Size(870, 270);
            this.dgvSchedule.TabIndex = 0;

            // lblDay
            this.lblDay.AutoSize = true;
            this.lblDay.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblDay.ForeColor = Color.White;
            this.lblDay.Location = new Point(10, 10);
            this.lblDay.Size = new Size(0, 20);
            this.lblDay.TabIndex = 1;

            // lblTotalHours
            this.lblTotalHours.AutoSize = true;
            this.lblTotalHours.Font = new Font("Segoe UI", 10F);
            this.lblTotalHours.ForeColor = Color.White;
            this.lblTotalHours.Location = new Point(10, 360);
            this.lblTotalHours.Size = new Size(0, 17);
            this.lblTotalHours.TabIndex = 2;

            // lblWeeklyHours
            this.lblWeeklyHours.AutoSize = true;
            this.lblWeeklyHours.Font = new Font("Segoe UI", 10F);
            this.lblWeeklyHours.ForeColor = Color.White;
            this.lblWeeklyHours.Location = new Point(10, 390);
            this.lblWeeklyHours.Size = new Size(0, 17);
            this.lblWeeklyHours.TabIndex = 3;

            // btnSaveNotes
            this.btnSaveNotes.Location = new Point(690, 360);
            this.btnSaveNotes.Size = new Size(100, 30);
            this.btnSaveNotes.TabIndex = 4;
            this.btnSaveNotes.Text = "Сохранить";
            this.btnSaveNotes.BackColor = Color.FromArgb(50, 50, 50);
            this.btnSaveNotes.ForeColor = Color.White;
            this.btnSaveNotes.FlatStyle = FlatStyle.Flat;
            this.btnSaveNotes.FlatAppearance.BorderSize = 0;
            this.btnSaveNotes.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            this.btnSaveNotes.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
            this.btnSaveNotes.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.btnSaveNotes.Click += new EventHandler(this.BtnSaveNotes_Click);

            // btnBack
            this.btnBack.Location = new Point(690, 330);
            this.btnBack.Size = new Size(100, 30);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "Назад";
            this.btnBack.BackColor = Color.FromArgb(50, 50, 50);
            this.btnBack.ForeColor = Color.White;
            this.btnBack.FlatStyle = FlatStyle.Flat;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            this.btnBack.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
            this.btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.btnBack.Click += new EventHandler(this.BtnBack_Click);

            // btnPrevDay
            this.btnPrevDay.Location = new Point(200, 10);
            this.btnPrevDay.Size = new Size(100, 30);
            this.btnPrevDay.TabIndex = 6;
            this.btnPrevDay.Text = "Пред. день";
            this.btnPrevDay.BackColor = Color.FromArgb(50, 50, 50);
            this.btnPrevDay.ForeColor = Color.White;
            this.btnPrevDay.FlatStyle = FlatStyle.Flat;
            this.btnPrevDay.FlatAppearance.BorderSize = 0;
            this.btnPrevDay.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            this.btnPrevDay.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
            this.btnPrevDay.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.btnPrevDay.Click += new EventHandler(this.BtnPrevDay_Click);

            // btnNextDay
            this.btnNextDay.Location = new Point(310, 10);
            this.btnNextDay.Size = new Size(100, 30);
            this.btnNextDay.TabIndex = 7;
            this.btnNextDay.Text = "След. день";
            this.btnNextDay.BackColor = Color.FromArgb(50, 50, 50);
            this.btnNextDay.ForeColor = Color.White;
            this.btnNextDay.FlatStyle = FlatStyle.Flat;
            this.btnNextDay.FlatAppearance.BorderSize = 0;
            this.btnNextDay.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            this.btnNextDay.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
            this.btnNextDay.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.btnNextDay.Click += new EventHandler(this.BtnNextDay_Click);

            // btnGenerateReport
            this.btnGenerateReport.Location = new Point(690, 390);
            this.btnGenerateReport.Size = new Size(100, 30);
            this.btnGenerateReport.TabIndex = 8;
            this.btnGenerateReport.Text = "Сформировать отчет";
            this.btnGenerateReport.BackColor = Color.FromArgb(50, 50, 50);
            this.btnGenerateReport.ForeColor = Color.White;
            this.btnGenerateReport.FlatStyle = FlatStyle.Flat;
            this.btnGenerateReport.FlatAppearance.BorderSize = 0;
            this.btnGenerateReport.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            this.btnGenerateReport.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
            this.btnGenerateReport.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.btnGenerateReport.Click += new EventHandler(this.BtnGenerateReport_Click);

            // DayScheduleForm
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ClientSize = new Size(900, 430);
            this.Controls.Add(this.dgvSchedule);
            this.Controls.Add(this.lblDay);
            this.Controls.Add(this.lblTotalHours);
            this.Controls.Add(this.lblWeeklyHours);
            this.Controls.Add(this.btnSaveNotes);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnPrevDay);
            this.Controls.Add(this.btnNextDay);
            this.Controls.Add(this.btnGenerateReport);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "Расписание на день";
            this.FormClosing += new FormClosingEventHandler(this.DayScheduleForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedule)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
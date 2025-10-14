using System.Windows.Forms;
using System;
using System.Drawing;

namespace Diary3
{
    partial class ReportForm
    {
        private System.ComponentModel.IContainer components = null;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private ComboBox cmbReportType;
        private ComboBox cmbExportFormat;
        private Button btnGenerate;
        private Button btnCancel;
        private Label lblStartDate;
        private Label lblEndDate;
        private Label lblReportType;
        private Label lblExportFormat;

        private void InitializeComponent()
        {
            this.dtpStartDate = new DateTimePicker();
            this.dtpEndDate = new DateTimePicker();
            this.cmbReportType = new ComboBox();
            this.cmbExportFormat = new ComboBox();
            this.btnGenerate = new Button();
            this.btnCancel = new Button();
            this.lblStartDate = new Label();
            this.lblEndDate = new Label();
            this.lblReportType = new Label();
            this.lblExportFormat = new Label();

            this.SuspendLayout();

            // lblStartDate
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new Point(20, 20);
            this.lblStartDate.Text = "Дата начала:";
            this.lblStartDate.ForeColor = Color.White;
            this.lblStartDate.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // dtpStartDate
            this.dtpStartDate.Location = new Point(120, 20);
            this.dtpStartDate.Size = new Size(200, 20);
            this.dtpStartDate.BackColor = Color.FromArgb(45, 45, 45);
            this.dtpStartDate.ForeColor = Color.White;

            // lblEndDate
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new Point(20, 50);
            this.lblEndDate.Text = "Дата окончания:";
            this.lblEndDate.ForeColor = Color.White;
            this.lblEndDate.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // dtpEndDate
            this.dtpEndDate.Location = new Point(120, 50);
            this.dtpEndDate.Size = new Size(200, 20);
            this.dtpEndDate.BackColor = Color.FromArgb(45, 45, 45);
            this.dtpEndDate.ForeColor = Color.White;

            // lblReportType
            this.lblReportType.AutoSize = true;
            this.lblReportType.Location = new Point(20, 80);
            this.lblReportType.Text = "Тип отчета:";
            this.lblReportType.ForeColor = Color.White;
            this.lblReportType.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // cmbReportType
            this.cmbReportType.Location = new Point(120, 80);
            this.cmbReportType.Size = new Size(200, 20);
            this.cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbReportType.BackColor = Color.FromArgb(45, 45, 45);
            this.cmbReportType.ForeColor = Color.White;
            this.cmbReportType.FlatStyle = FlatStyle.Flat;

            // lblExportFormat
            this.lblExportFormat.AutoSize = true;
            this.lblExportFormat.Location = new Point(20, 110);
            this.lblExportFormat.Text = "Формат экспорта:";
            this.lblExportFormat.ForeColor = Color.White;
            this.lblExportFormat.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // cmbExportFormat
            this.cmbExportFormat.Location = new Point(120, 110);
            this.cmbExportFormat.Size = new Size(200, 20);
            this.cmbExportFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbExportFormat.BackColor = Color.FromArgb(45, 45, 45);
            this.cmbExportFormat.ForeColor = Color.White;
            this.cmbExportFormat.FlatStyle = FlatStyle.Flat;

            // btnGenerate
            this.btnGenerate.Location = new Point(120, 150);
            this.btnGenerate.Size = new Size(100, 30);
            this.btnGenerate.Text = "Сформировать";
            this.btnGenerate.BackColor = Color.FromArgb(50, 50, 50);
            this.btnGenerate.ForeColor = Color.White;
            this.btnGenerate.FlatStyle = FlatStyle.Flat;
            this.btnGenerate.FlatAppearance.BorderSize = 0;
            this.btnGenerate.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            this.btnGenerate.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
            this.btnGenerate.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.btnGenerate.Click += new EventHandler(this.BtnGenerate_Click);

            // btnCancel
            this.btnCancel.Location = new Point(230, 150);
            this.btnCancel.Size = new Size(100, 30);
            this.btnCancel.Text = "Отмена";
            this.btnCancel.BackColor = Color.FromArgb(50, 50, 50);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            this.btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
            this.btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.btnCancel.Click += new EventHandler(this.BtnCancel_Click);

            // ReportForm
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ClientSize = new Size(350, 200);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.cmbReportType);
            this.Controls.Add(this.cmbExportFormat);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.lblReportType);
            this.Controls.Add(this.lblExportFormat);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "Формирование отчета";
            this.FormClosing += new FormClosingEventHandler(this.ReportForm_FormClosing);
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
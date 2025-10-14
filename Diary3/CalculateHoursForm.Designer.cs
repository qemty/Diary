using System.Drawing;
using System.Windows.Forms;

namespace Diary3
{
    partial class CalculateHoursForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvHours1;
        private DataGridView dgvHours2;
        private Label lblTotalHours;
        private Button btnCalculate;
        private DateTimePicker dtpStartDate; // Новый элемент для начальной даты
        private DateTimePicker dtpEndDate;   // Новый элемент для конечной даты
        private Label lblStartDate;          // Метка для начальной даты
        private Label lblEndDate;            // Метка для конечной даты
        private ComboBox cbYear;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dgvHours1 = new System.Windows.Forms.DataGridView();
            this.dgvHours2 = new System.Windows.Forms.DataGridView();
            this.lblTotalHours = new System.Windows.Forms.Label();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.lblEndDate = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvHours1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHours2)).BeginInit();
            this.SuspendLayout();

            // dgvHours1
            this.dgvHours1.BackgroundColor = Color.FromArgb(45, 45, 45);
            this.dgvHours1.ForeColor = Color.White;
            this.dgvHours1.GridColor = Color.Gray;
            this.dgvHours1.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
            this.dgvHours1.DefaultCellStyle.ForeColor = Color.White;
            this.dgvHours1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            this.dgvHours1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvHours1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.dgvHours1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            this.dgvHours1.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvHours1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHours1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Number", HeaderText = "№", ReadOnly = true, Width = 50 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Дата", Width = 200 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Hours", HeaderText = "Часы", Width = 150 }
            });
            this.dgvHours1.Location = new System.Drawing.Point(10, 70); // Сдвигаем вниз, чтобы уместить DateTimePicker
            this.dgvHours1.Name = "dgvHours1";
            this.dgvHours1.Size = new System.Drawing.Size(750, 740);
            this.dgvHours1.TabIndex = 0;
            for (int i = 1; i <= 32; i++)
            {
                this.dgvHours1.Rows.Add(i, "", "0");
            }

            // dgvHours2
            this.dgvHours2.BackgroundColor = Color.FromArgb(45, 45, 45);
            this.dgvHours2.ForeColor = Color.White;
            this.dgvHours2.GridColor = Color.Gray;
            this.dgvHours2.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
            this.dgvHours2.DefaultCellStyle.ForeColor = Color.White;
            this.dgvHours2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            this.dgvHours2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvHours2.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.dgvHours2.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            this.dgvHours2.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvHours2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHours2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Number", HeaderText = "№", ReadOnly = true, Width = 50 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Дата", Width = 200 },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Hours", HeaderText = "Часы", Width = 150 }
            });
            this.dgvHours2.Location = new System.Drawing.Point(770, 70); // Сдвигаем вниз
            this.dgvHours2.Name = "dgvHours2";
            this.dgvHours2.Size = new System.Drawing.Size(750, 740);
            this.dgvHours2.TabIndex = 1;
            for (int i = 33; i <= 64; i++)
            {
                this.dgvHours2.Rows.Add(i, "", "0");
            }

            // lblStartDate
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblStartDate.ForeColor = Color.White;
            this.lblStartDate.Location = new System.Drawing.Point(10, 10);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Text = "Дата начала:";

            // dtpStartDate
            this.dtpStartDate.Location = new System.Drawing.Point(100, 10);
            this.dtpStartDate.Size = new System.Drawing.Size(200, 20);
            this.dtpStartDate.BackColor = Color.FromArgb(45, 45, 45);
            this.dtpStartDate.ForeColor = Color.White;
            this.dtpStartDate.Format = DateTimePickerFormat.Short;

            // lblEndDate
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblEndDate.ForeColor = Color.White;
            this.lblEndDate.Location = new System.Drawing.Point(310, 10);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Text = "Дата окончания:";

            // dtpEndDate
            this.dtpEndDate.Location = new System.Drawing.Point(425, 10);
            this.dtpEndDate.Size = new System.Drawing.Size(200, 20);
            this.dtpEndDate.BackColor = Color.FromArgb(45, 45, 45);
            this.dtpEndDate.ForeColor = Color.White;
            this.dtpEndDate.Format = DateTimePickerFormat.Short;

            // lblTotalHours
            this.lblTotalHours.AutoSize = true;
            this.lblTotalHours.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);
            this.lblTotalHours.ForeColor = Color.White;
            this.lblTotalHours.Location = new System.Drawing.Point(10, 820); // Сдвигаем ниже
            this.lblTotalHours.Name = "lblTotalHours";
            this.lblTotalHours.Size = new System.Drawing.Size(0, 20);
            this.lblTotalHours.TabIndex = 2;

            // btnCalculate
            this.btnCalculate.Location = new System.Drawing.Point(1420, 820); // Сдвигаем ниже
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(100, 30);
            this.btnCalculate.TabIndex = 3;
            this.btnCalculate.Text = "Рассчитать";
            this.btnCalculate.BackColor = Color.FromArgb(50, 50, 50);
            this.btnCalculate.ForeColor = Color.White;
            this.btnCalculate.FlatStyle = FlatStyle.Flat;
            this.btnCalculate.FlatAppearance.BorderSize = 0;
            this.btnCalculate.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            this.btnCalculate.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
            this.btnCalculate.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.btnCalculate.Click += new System.EventHandler(this.BtnCalculate_Click);

            // cbYear
            this.cbYear = new System.Windows.Forms.ComboBox();
            this.cbYear.Location = new System.Drawing.Point(630, 10);
            this.cbYear.Size = new System.Drawing.Size(100, 20);
            this.cbYear.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbYear.BackColor = Color.FromArgb(45, 45, 45);
            this.cbYear.ForeColor = Color.White;
            this.cbYear.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.cbYear.Items.AddRange(new object[] { "2024", "2025", "2026" });
            this.cbYear.SelectedIndex = 1; // По умолчанию 2025
            this.cbYear.SelectedIndexChanged += new System.EventHandler(this.CbYear_SelectedIndexChanged);

            // CalculateHoursForm
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.ClientSize = new System.Drawing.Size(1534, 880);
            this.Controls.Add(this.dgvHours1);
            this.Controls.Add(this.dgvHours2);
            this.Controls.Add(this.lblTotalHours);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.cbYear);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "CalculateHoursForm";
            this.Text = "Рассчитать проведенные часы";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CalculateHoursForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHours1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHours2)).EndInit();
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
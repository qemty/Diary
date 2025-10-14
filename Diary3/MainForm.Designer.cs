using System.Windows.Forms;
using System.Drawing;

namespace Diary3
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lblMonday = new System.Windows.Forms.Label();
            this.lblTuesday = new System.Windows.Forms.Label();
            this.lblWednesday = new System.Windows.Forms.Label();
            this.lblThursday = new System.Windows.Forms.Label();
            this.lblFriday = new System.Windows.Forms.Label();
            this.lblSaturday = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblMonday
            this.lblMonday.AutoSize = true;
            this.lblMonday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMonday.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular);
            this.lblMonday.ForeColor = Color.White;
            this.lblMonday.Location = new System.Drawing.Point(113, 52);
            this.lblMonday.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMonday.Name = "lblMonday";
            this.lblMonday.Size = new System.Drawing.Size(54, 28);
            this.lblMonday.TabIndex = 0;
            this.lblMonday.Text = "Пнд";

            // lblTuesday
            this.lblTuesday.AutoSize = true;
            this.lblTuesday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblTuesday.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular);
            this.lblTuesday.ForeColor = Color.White;
            this.lblTuesday.Location = new System.Drawing.Point(352, 52);
            this.lblTuesday.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTuesday.Name = "lblTuesday";
            this.lblTuesday.Size = new System.Drawing.Size(47, 28);
            this.lblTuesday.TabIndex = 1;
            this.lblTuesday.Text = "Втр";

            // lblWednesday
            this.lblWednesday.AutoSize = true;
            this.lblWednesday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblWednesday.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular);
            this.lblWednesday.ForeColor = Color.White;
            this.lblWednesday.Location = new System.Drawing.Point(595, 52);
            this.lblWednesday.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWednesday.Name = "lblWednesday";
            this.lblWednesday.Size = new System.Drawing.Size(52, 28);
            this.lblWednesday.TabIndex = 2;
            this.lblWednesday.Text = "Срд";

            // lblThursday
            this.lblThursday.AutoSize = true;
            this.lblThursday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblThursday.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular);
            this.lblThursday.ForeColor = Color.White;
            this.lblThursday.Location = new System.Drawing.Point(843, 52);
            this.lblThursday.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThursday.Name = "lblThursday";
            this.lblThursday.Size = new System.Drawing.Size(48, 28);
            this.lblThursday.TabIndex = 3;
            this.lblThursday.Text = "Чтв";

            // lblFriday
            this.lblFriday.AutoSize = true;
            this.lblFriday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblFriday.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular);
            this.lblFriday.ForeColor = Color.White;
            this.lblFriday.Location = new System.Drawing.Point(1078, 52);
            this.lblFriday.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFriday.Name = "lblFriday";
            this.lblFriday.Size = new System.Drawing.Size(50, 28);
            this.lblFriday.TabIndex = 4;
            this.lblFriday.Text = "Птн";

            // lblSaturday
            this.lblSaturday.AutoSize = true;
            this.lblSaturday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblSaturday.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular);
            this.lblSaturday.ForeColor = Color.White;
            this.lblSaturday.Location = new System.Drawing.Point(1313, 52);
            this.lblSaturday.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSaturday.Name = "lblSaturday";
            this.lblSaturday.Size = new System.Drawing.Size(48, 28);
            this.lblSaturday.TabIndex = 5;
            this.lblSaturday.Text = "Сбт";

            // button1
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(1406, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 34);
            this.button1.TabIndex = 6;
            this.button1.Text = "Расчет часов";
            this.button1.BackColor = Color.FromArgb(50, 50, 50);
            this.button1.ForeColor = Color.White;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            this.button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(60, 60, 60);
            this.ClientSize = new System.Drawing.Size(1534, 821);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblSaturday);
            this.Controls.Add(this.lblFriday);
            this.Controls.Add(this.lblThursday);
            this.Controls.Add(this.lblWednesday);
            this.Controls.Add(this.lblTuesday);
            this.Controls.Add(this.lblMonday);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular);
            this.ForeColor = Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Расписание";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblMonday;
        private System.Windows.Forms.Label lblTuesday;
        private System.Windows.Forms.Label lblWednesday;
        private System.Windows.Forms.Label lblThursday;
        private System.Windows.Forms.Label lblFriday;
        private System.Windows.Forms.Label lblSaturday;
        private Button button1;
    }
}
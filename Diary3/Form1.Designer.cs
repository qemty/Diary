using System.Windows.Forms;
using System.Drawing;

namespace Diary3
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.helpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // txtLastName
            this.txtLastName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);
            this.txtLastName.Location = new System.Drawing.Point(274, 188);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(251, 29);
            this.txtLastName.TabIndex = 0;
            this.txtLastName.BackColor = Color.FromArgb(45, 45, 45);
            this.txtLastName.ForeColor = Color.White;
            this.txtLastName.BorderStyle = BorderStyle.FixedSingle;

            // txtPassword
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);
            this.txtPassword.Location = new System.Drawing.Point(274, 250);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(251, 29);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.BackColor = Color.FromArgb(45, 45, 45);
            this.txtPassword.ForeColor = Color.White;
            this.txtPassword.BorderStyle = BorderStyle.FixedSingle;

            // button1
            this.button1.BackColor = Color.FromArgb(50, 50, 50);
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            this.button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);
            this.button1.ForeColor = Color.White;
            this.button1.Location = new System.Drawing.Point(681, 390);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 39);
            this.button1.TabIndex = 2;
            this.button1.Text = "Войти";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnLogin_Click);

            // label1
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);
            this.label1.ForeColor = Color.White;
            this.label1.Location = new System.Drawing.Point(270, 164);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "Фамилия И.О.";

            // label2
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);
            this.label2.ForeColor = Color.White;
            this.label2.Location = new System.Drawing.Point(270, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "Пароль";

            // helpBtn
            this.helpBtn.BackColor = Color.FromArgb(50, 50, 50);
            this.helpBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("helpBtn.BackgroundImage")));
            this.helpBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.helpBtn.Cursor = System.Windows.Forms.Cursors.Help;
            this.helpBtn.FlatAppearance.BorderSize = 0;
            this.helpBtn.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            this.helpBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
            this.helpBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpBtn.Location = new System.Drawing.Point(12, 12);
            this.helpBtn.Name = "helpBtn";
            this.helpBtn.Size = new System.Drawing.Size(33, 27);
            this.helpBtn.TabIndex = 5;
            this.helpBtn.UseVisualStyleBackColor = false;
            this.helpBtn.Click += new System.EventHandler(this.helpBtn_Click);

            // LoginForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.helpBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtLastName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Авторизация";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button button1;
        private Label label1;
        private Label label2;
        private Button helpBtn;
    }
}
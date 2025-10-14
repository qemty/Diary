using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace Diary3
{
    public partial class TeacherEditForm : Form
    {
        private TextBox txtId; // Добавляем поле для Id
        private TextBox txtName;
        private TextBox txtPassword;
        private ComboBox cmbRole;
        private Button btnSave;
        private Button btnCancel;

        public int TeacherId => int.Parse(txtId.Text); // Свойство для Id
        public string TeacherName => txtName.Text;
        public string TeacherPassword => txtPassword.Text;
        public Role TeacherRole => (Role)Enum.Parse(typeof(Role), cmbRole.SelectedItem.ToString());

        public TeacherEditForm(Teacher teacher)
        {
            InitializeComponent();
            InitializeControls();

            // Если редактируем существующего преподавателя, заполняем поля
            if (teacher != null)
            {
                txtId.Text = teacher.Id.ToString();
                txtId.Enabled = true; // Разрешаем редактировать Id
                txtName.Text = teacher.Name;
                txtPassword.Text = teacher.Password;
                cmbRole.SelectedItem = teacher.Role.ToString();
            }
            else
            {
                txtId.Enabled = true; // Разрешаем ввод Id для нового преподавателя
            }
        }

        private void InitializeControls()
        {
            this.Text = "Редактирование преподавателя";
            this.Size = new System.Drawing.Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.ForeColor = Color.White;

            // Поле для Id
            var lblId = new Label
            {
                Text = "ID:",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(100, 20)
            };
            txtId = new TextBox
            {
                Location = new System.Drawing.Point(120, 20),
                Size = new System.Drawing.Size(200, 20),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(lblId);
            this.Controls.Add(txtId);

            // Поле для имени
            var lblName = new Label
            {
                Text = "Фамилия И.О.:",
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(100, 20)
            };
            txtName = new TextBox
            {
                Location = new System.Drawing.Point(120, 60),
                Size = new System.Drawing.Size(200, 20),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);

            // Поле для пароля
            var lblPassword = new Label
            {
                Text = "Пароль:",
                Location = new System.Drawing.Point(20, 100),
                Size = new System.Drawing.Size(100, 20)
            };
            txtPassword = new TextBox
            {
                Location = new System.Drawing.Point(120, 100),
                Size = new System.Drawing.Size(200, 20),
                UseSystemPasswordChar = true,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);

            // Поле для роли
            var lblRole = new Label
            {
                Text = "Роль:",
                Location = new System.Drawing.Point(20, 140),
                Size = new System.Drawing.Size(100, 20)
            };
            cmbRole = new ComboBox
            {
                Location = new System.Drawing.Point(120, 140),
                Size = new System.Drawing.Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            cmbRole.Items.AddRange(new[] { Role.Teacher.ToString(), Role.Admin.ToString() });
            cmbRole.SelectedIndex = 0; // По умолчанию - Teacher
            this.Controls.Add(lblRole);
            this.Controls.Add(cmbRole);

            // Кнопка "Сохранить"
            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new System.Drawing.Point(120, 180),
                Size = new System.Drawing.Size(100, 30),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0, MouseDownBackColor = Color.FromArgb(70, 70, 70), MouseOverBackColor = Color.FromArgb(70, 70, 70) },
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            btnSave.Click += (s, e) =>
            {
                // Проверяем, что все поля заполнены
                if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверяем, что Id - это число
                if (!int.TryParse(txtId.Text, out int id) || id <= 0)
                {
                    MessageBox.Show("Id должен быть положительным числом!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            this.Controls.Add(btnSave);

            // Кнопка "Отмена"
            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new System.Drawing.Point(230, 180),
                Size = new System.Drawing.Size(100, 30),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0, MouseDownBackColor = Color.FromArgb(70, 70, 70), MouseOverBackColor = Color.FromArgb(70, 70, 70) },
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };
            this.Controls.Add(btnCancel);
        }
    }
}
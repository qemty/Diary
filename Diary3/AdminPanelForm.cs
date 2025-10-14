using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Diary3;
using HtmlAgilityPack;


namespace Diary3
{
    public partial class AdminPanelForm : Form
    {
        private DataGridView dgvTeachers;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnLogout;

        public AdminPanelForm()
        {
            InitializeComponent();
            InitializeControls();
            LoadTeachers();
        }

        private void InitializeControls()
        {
            this.Text = "Панель администратора - Управление преподавателями";
            this.Size = new System.Drawing.Size(800, 600);
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // DataGridView для отображения списка преподавателей
            dgvTeachers = new DataGridView
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(760, 400),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                GridColor = Color.Gray,
                DefaultCellStyle = { BackColor = Color.FromArgb(45, 45, 45), ForeColor = Color.White },
                ColumnHeadersDefaultCellStyle = { BackColor = Color.FromArgb(50, 50, 50), ForeColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Bold) },
                RowHeadersDefaultCellStyle = { BackColor = Color.FromArgb(50, 50, 50), ForeColor = Color.White }
            };
            dgvTeachers.Columns.Add("Id", "ID");
            dgvTeachers.Columns.Add("Name", "Фамилия И.О.");
            dgvTeachers.Columns.Add("Password", "Пароль");
            dgvTeachers.Columns.Add("Role", "Роль");
            this.Controls.Add(dgvTeachers);

            // Кнопка "Добавить"
            btnAdd = new Button
            {
                Text = "Добавить преподавателя",
                Location = new System.Drawing.Point(10, 420),
                Size = new System.Drawing.Size(150, 30),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0, MouseDownBackColor = Color.FromArgb(70, 70, 70), MouseOverBackColor = Color.FromArgb(70, 70, 70) },
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            // Кнопка "Редактировать"
            btnEdit = new Button
            {
                Text = "Редактировать",
                Location = new System.Drawing.Point(170, 420),
                Size = new System.Drawing.Size(150, 30),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0, MouseDownBackColor = Color.FromArgb(70, 70, 70), MouseOverBackColor = Color.FromArgb(70, 70, 70) },
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            btnEdit.Click += BtnEdit_Click;
            this.Controls.Add(btnEdit);

            // Кнопка "Удалить"
            btnDelete = new Button
            {
                Text = "Удалить",
                Location = new System.Drawing.Point(330, 420),
                Size = new System.Drawing.Size(150, 30),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0, MouseDownBackColor = Color.FromArgb(70, 70, 70), MouseOverBackColor = Color.FromArgb(70, 70, 70) },
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            // Кнопка "Выйти"
            btnLogout = new Button
            {
                Text = "Выйти",
                Location = new System.Drawing.Point(620, 420),
                Size = new System.Drawing.Size(150, 30),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0, MouseDownBackColor = Color.FromArgb(70, 70, 70), MouseOverBackColor = Color.FromArgb(70, 70, 70) },
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            btnLogout.Click += BtnLogout_Click;
            this.Controls.Add(btnLogout);
        }

        private void LoadTeachers()
        {
            using (var db = new Datab())
            {
                var teachers = db.Teacher.ToList();
                dgvTeachers.Rows.Clear();
                foreach (var teacher in teachers)
                {
                    dgvTeachers.Rows.Add(teacher.Id, teacher.Name, teacher.Password, teacher.Role.ToString());
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new TeacherEditForm(null))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    using (var db = new Datab())
                    {
                        // Проверяем, не существует ли уже преподаватель с таким Id
                        if (db.Teacher.Any(t => t.Id == form.TeacherId))
                        {
                            MessageBox.Show("Преподаватель с таким Id уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var newTeacher = new Teacher
                        {
                            Id = form.TeacherId,
                            Name = form.TeacherName,
                            Password = form.TeacherPassword,
                            Role = form.TeacherRole
                        };
                        db.Teacher.Add(newTeacher);
                        Console.WriteLine($"Adding new teacher: Id={newTeacher.Id}, Name={newTeacher.Name}");
                        db.SaveChanges();
                        LoadTeachers();
                    }
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTeachers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите преподавателя для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int teacherId = Convert.ToInt32(dgvTeachers.SelectedRows[0].Cells["Id"].Value);
            using (var db = new Datab())
            {
                var teacher = db.Teacher.FirstOrDefault(t => t.Id == teacherId);
                if (teacher == null)
                {
                    MessageBox.Show("Преподаватель не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var form = new TeacherEditForm(teacher))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // Проверяем, не занят ли новый Id
                        if (db.Teacher.Any(t => t.Id == form.TeacherId && t.Id != teacher.Id))
                        {
                            MessageBox.Show("Преподаватель с таким Id уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Если Id изменился, удаляем старую запись и создаем новую
                        if (teacher.Id != form.TeacherId)
                        {
                            // Создаем новую запись с новым Id
                            var newTeacher = new Teacher
                            {
                                Id = form.TeacherId,
                                Name = form.TeacherName,
                                Password = form.TeacherPassword,
                                Role = form.TeacherRole
                            };

                            Console.WriteLine($"Changing teacher Id from {teacher.Id} to {newTeacher.Id}, Name={newTeacher.Name}");

                            // Удаляем старую запись
                            db.Teacher.Remove(teacher);
                            // Добавляем новую запись
                            db.Teacher.Add(newTeacher);
                        }
                        else
                        {
                            // Если Id не изменился, просто обновляем остальные поля
                            teacher.Name = form.TeacherName;
                            teacher.Password = form.TeacherPassword;
                            teacher.Role = form.TeacherRole;
                            Console.WriteLine($"Updating teacher: Id={teacher.Id}, Name={teacher.Name}");
                        }

                        try
                        {
                            db.SaveChanges();
                            LoadTeachers();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Console.WriteLine($"Error saving changes: {ex.Message}");
                            if (ex.InnerException != null)
                            {
                                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                            }
                        }
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTeachers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите преподавателя для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int teacherId = Convert.ToInt32(dgvTeachers.SelectedRows[0].Cells["Id"].Value);
            if (MessageBox.Show("Вы уверены, что хотите удалить этого преподавателя? Все связанные уроки также будут удалены.", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (var db = new Datab())
                {
                    var teacher = db.Teacher.FirstOrDefault(t => t.Id == teacherId);
                    if (teacher != null)
                    {
                        db.Teacher.Remove(teacher);
                        db.SaveChanges();
                        LoadTeachers();
                    }
                }
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            var loginForm = new LoginForm();
            loginForm.Show();
        }
    }
}
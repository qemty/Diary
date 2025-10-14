using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Diary3
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            UpdateTeacherRoles();
        }

        // Метод для обновления ролей в базе данных
        private void UpdateTeacherRoles()
        {
            using (var context = new Datab())
            {
                // Проверяем, есть ли записи с Role = NULL
                var teachersWithNullRole = context.Teacher
                    .Where(t => t.Role == null)
                    .ToList();

                if (teachersWithNullRole.Any())
                {
                    // Выполняем SQL-запрос для обновления
                    context.Database.ExecuteSqlCommand(
                        "UPDATE Teachers SET Role = 0 WHERE Role IS NULL"
                    );
                    Console.WriteLine("Updated Roles for existing teachers to Teacher (0).");
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Обновляем существующих преподавателей, если у них не указана роль
            using (var context = new Datab())
            {
                var teachersWithoutRole = context.Teacher.Where(t => t.Role == 0).ToList();
                foreach (var teacher in teachersWithoutRole)
                {
                    teacher.Role = Role.Teacher; // Устанавливаем роль Teacher для существующих записей
                }
                context.SaveChanges();
            }

            string lastName = txtLastName.Text;
            string password = txtPassword.Text;

            // Проверка на администратора
            if (lastName == "administrator" && password == "AdminPoit2025")
            {
                var admin = new Teacher
                {
                    Name = "administrator",
                    Password = "AdminPoit2025",
                    Role = Role.Admin
                };
                AdminPanelForm adminForm = new AdminPanelForm();
                this.Hide();
                adminForm.ShowDialog();
                this.Close();
                return;
            }

            // Обычная авторизация
            using (var context = new Datab())
            {
                try
                {
                    if (string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(password))
                    {
                        throw new Exception("Заполните поля!");
                    }

                    var user = context.Teacher.FirstOrDefault(u => u.Name == lastName && u.Password == password);
                    if (user != null)
                    {
                        if (user.Role == Role.Teacher)
                        {
                            MainForm teacherForm = new MainForm(user);
                            this.Hide();
                            teacherForm.ShowDialog();
                            this.Close();
                        }
                        else if (user.Role == Role.Admin)
                        {
                            AdminPanelForm adminForm = new AdminPanelForm();
                            this.Hide();
                            adminForm.ShowDialog();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Фамилия или пароль введены неверно!", "Error", MessageBoxButtons.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                }
            }
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            string helpFilePath = @"help_diary.chm";

            if (System.IO.File.Exists(helpFilePath))
            {
                Process.Start(helpFilePath);
            }
            else
            {
                MessageBox.Show("Файл справки не найден.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
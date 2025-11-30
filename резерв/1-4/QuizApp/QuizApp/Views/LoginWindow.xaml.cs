using QuizApp.Data;
using QuizApp.Models;
using QuizApp.Services;
using QuizApp.Views;
using System;
using System.Windows;

namespace QuizApp.Views
{
    public partial class LoginWindow : Window
    {
        private readonly UserService _userService;
        public LoginWindow()
        {
            InitializeComponent();

            var factory = new SqlRepositoryFactory();

            var userRepo = factory.CreateUserRepository();
            _userService = new UserService(userRepo);
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            var reg = new RegisterWindow();
            reg.ShowDialog();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBox.Text.Trim();
            var pass = PasswordBox.Password;

            User user = null;

            if (login == "admin" && pass == "1111")
            {
                user = new User
                {
                    UserId = 0,
                    Login = "Admin",
                    BirthDate = DateTime.Today
                };
                var adminWin = new AdminWindow();
                adminWin.Show();
                this.Close();
                return;
            }
            else
            {
                user = _userService.Login(login, pass);
            }

            if (user == null)
            {
                MessageBox.Show("Невірний логін або пароль.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var main = new MainWindow(user);
            main.Show();
            this.Close();
        }
    }
}
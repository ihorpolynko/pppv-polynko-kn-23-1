using System;
using System.Windows;
using QuizApp.Data;
using QuizApp.Services;

namespace QuizApp.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly UserService _userService;
        public RegisterWindow()
        {
            InitializeComponent();

            var factory = new SqlRepositoryFactory();

            var userRepo = factory.CreateUserRepository();

            _userService = new UserService(userRepo);

            BirthPicker.SelectedDate = DateTime.Now;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginBox.Text.Trim();
            var pass = PasswordBox.Password;
            var bd = BirthPicker.SelectedDate ?? DateTime.Now;

            if (_userService.Register(login, pass, bd, out string err))
            {
                MessageBox.Show("Реєстрація пройшла успішно! Можно увійти.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show(err ?? "Помилка реєстрації", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
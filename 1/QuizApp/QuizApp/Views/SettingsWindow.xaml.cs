using System;
using System.Windows;
using QuizApp.Models;
using QuizApp.Services;
using QuizApp.Data;

namespace QuizApp.Views
{
    public partial class SettingsWindow : Window
    {
        private readonly User _user;
        private readonly UserService _userService;

        public SettingsWindow(User user)
        {
            InitializeComponent();
            _user = user;

            var userRepo = new SqlUserRepository(DbConfig.ConnectionString);
            _userService = new UserService(userRepo);

            BirthDatePicker.SelectedDate = _user.BirthDate;
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string current = CurrentPasswordBox.Password;
            string newPass = NewPasswordBox.Password;

            if (_userService.ChangePassword(_user.UserId, current, newPass, out string error))
            {
                MessageBox.Show("Пароль успішно змінено!");
                CurrentPasswordBox.Clear();
                NewPasswordBox.Clear();
            }
            else
            {
                MessageBox.Show($"Помилка: {error}");
            }
        }

        private void BtnChangeBirthDate_Click(object sender, RoutedEventArgs e)
        {
            if (BirthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Виберіть дату.");
                return;
            }

            if (_userService.ChangeBirthDate(_user.UserId, BirthDatePicker.SelectedDate.Value, out string error))
            {
                MessageBox.Show("Дата народження успішно змінена!");
                _user.BirthDate = BirthDatePicker.SelectedDate.Value;
            }
            else
            {
                MessageBox.Show($"Помилка: {error}");
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
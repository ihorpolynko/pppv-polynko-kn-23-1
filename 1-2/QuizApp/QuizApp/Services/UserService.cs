using System;
using QuizApp.Data;
using QuizApp.Models;

namespace QuizApp.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public bool Register(string login, string password, DateTime birthDate, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                error = "Логін і пароль обов'язкові.";
                return false;
            }
            var exists = _repo.GetByLogin(login);
            if (exists != null)
            {
                error = "Користувач з таким логіном вже існує.";
                return false;
            }
            var user = new User
            {
                Login = login,
                Password = password,
                BirthDate = birthDate
            };
            _repo.Add(user);
            return true;
        }

        public User Login(string login, string password)
        {
            var user = _repo.GetByLogin(login);
            if (user == null) return null;
            if (user.Password == password) return user;

            return null;
        }

        public bool ChangePassword(int userId, string currentPassword, string newPassword, out string error)
        {
            error = null;
            var user = _repo.GetById(userId);
            if (user == null)
            {
                error = "Користувача не знайдено.";
                return false;
            }

            if (user.Password != currentPassword)
            {
                error = "Поточний пароль невірний.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                error = "Новий пароль не може бути порожнім.";
                return false;
            }

            user.Password = newPassword;
            _repo.Update(user);
            return true;
        }

        public bool ChangeBirthDate(int userId, DateTime newBirthDate, out string error)
        {
            error = null;
            var user = _repo.GetById(userId);
            if (user == null)
            {
                error = "Користувача не знайдено.";
                return false;
            }

            user.BirthDate = newBirthDate;
            _repo.Update(user);
            return true;
        }
    }
}
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Data
{
    public interface IUserRepository
    {
        User GetByLogin(string login);
        void Add(User user);
        void Update(User user);
        User GetById(int id);
    }
}

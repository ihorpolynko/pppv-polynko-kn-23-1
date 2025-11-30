using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Data
{
    public interface IQuizRepository
    {
        Quiz GetById(int id);
        List<Quiz> GetAll();
        void Add(Quiz quiz);
        void Update(Quiz quiz);
        void Delete(int quizId);
    }
}
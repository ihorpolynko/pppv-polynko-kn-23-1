using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Data
{
    public interface IResultRepository
    {
        void Save(Result result);
        List<Result> GetByUser(int userId);
        List<Result> GetTopForQuiz(int quizId, int topN);
    }
}
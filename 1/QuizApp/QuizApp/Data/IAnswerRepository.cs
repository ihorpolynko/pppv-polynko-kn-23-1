using QuizApp.Models;
using System.Collections.Generic;

namespace QuizApp.Data
{
    public interface IAnswerRepository
    {
        List<Answer> GetByQuestion(int questionId);
        void Add(Answer answer);
        void Update(Answer answer);
        void Delete(int answerId);
        void DeleteByQuestion(int questionId);
    }
}
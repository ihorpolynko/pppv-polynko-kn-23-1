using QuizApp.Models;
using System.Collections.Generic;

namespace QuizApp.Data
{
    public interface IQuestionRepository
    {
        List<Question> GetByQuiz(int quizId);
        Question GetById(int questionId);
        void Add(Question question);
        void Update(Question question);
        void Delete(int questionId);
    }
}
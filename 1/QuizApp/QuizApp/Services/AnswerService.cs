using System.Collections.Generic;
using QuizApp.Data;
using QuizApp.Models;

namespace QuizApp.Services
{
    public class AnswerService
    {
        private readonly IAnswerRepository _repo;
        public AnswerService(IAnswerRepository repo) => _repo = repo;

        public List<Answer> GetAnswersByQuestion(int questionId) => _repo.GetByQuestion(questionId);

        public void AddAnswer(Answer answer) => _repo.Add(answer);
        public void UpdateAnswer(Answer answer) => _repo.Update(answer);
        public void DeleteAnswer(int answerId) => _repo.Delete(answerId);
    }
}
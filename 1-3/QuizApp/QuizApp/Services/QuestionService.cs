using System.Collections.Generic;
using System.Windows;
using QuizApp.Data;
using QuizApp.Models;

namespace QuizApp.Services
{
    public class QuestionService
    {
        private readonly IQuestionRepository _repo;
        private readonly IAnswerRepository _answerRepo;

        public QuestionService(IQuestionRepository repo, IAnswerRepository answerRepo)
        {
            _repo = repo;
            _answerRepo = answerRepo;
        }

        public List<Question> GetQuestionsByQuiz(int quizId)
        {
            var questions = _repo.GetByQuiz(quizId);
            foreach (var q in questions)
                q.Answers = _answerRepo.GetByQuestion(q.QuestionId);
            return questions;
        }

        public Question GetById(int questionId)
        {
            var question = _repo.GetById(questionId);
            if (question != null)
                question.Answers = _answerRepo.GetByQuestion(question.QuestionId);
            return question;
        }

        public void AddQuestion(Question question)
        {
            _repo.Add(question);
        }

        public void UpdateQuestion(Question question)
        {
            _repo.Update(question);
        }

        public void DeleteQuestion(int questionId)
        {
            _answerRepo.DeleteByQuestion(questionId);
            _repo.Delete(questionId);
        }
    }
}
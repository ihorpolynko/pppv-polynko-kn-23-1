using QuizApp.Data;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace QuizApp.Services
{
    public class QuizService
    {
        private readonly IQuizRepository _quizRepo;
        public QuizService(IQuizRepository repo)
        {
            _quizRepo = repo;
        }

        public List<Quiz> GetAllQuizzes() => _quizRepo.GetAll();

        public Quiz GetQuizWithQuestions(int id) => _quizRepo.GetById(id);


        public void AddQuiz(Quiz quiz) => _quizRepo.Add(quiz);
        public void UpdateQuiz(Quiz quiz) => _quizRepo.Update(quiz);
        public void DeleteQuiz(int quizId) => _quizRepo.Delete(quizId);

        public List<Question> GetRandomQuestions(int quizId, int upToCount = 20)
        {
            var quiz = _quizRepo.GetById(quizId);
            if (quiz == null) return new List<Question>();
            var rnd = new Random();
            return quiz.Questions.OrderBy(x => rnd.Next()).Take(upToCount).ToList();
        }

        public List<Question> GetMixedQuestions(int upToCount = 20)
        {
            var all = new List<Question>();
            var quizzes = _quizRepo.GetAll();
            foreach (var q in quizzes)
            {
                var full = _quizRepo.GetById(q.QuizId);
                if (full != null && full.Questions != null)
                {
                    all.AddRange(full.Questions);
                }
            }

            var rnd = new Random();
            return all.OrderBy(x => rnd.Next()).Take(upToCount).ToList();
        }

        public bool IsQuestionCorrect(Question question, List<int> chosenAnswerIds)
        {
            var correctIds = question.Answers.Where(a => a.IsCorrect).Select(a => a.AnswerId).OrderBy(x => x).ToList();
            chosenAnswerIds = (chosenAnswerIds ?? new List<int>()).OrderBy(x => x).ToList();
            return correctIds.SequenceEqual(chosenAnswerIds);
        }
    }
}
using System;
using System.Collections.Generic;
using QuizApp.Data;
using QuizApp.Models;

namespace QuizApp.Services
{
    public class ResultService
    {
        private readonly IResultRepository _repo;
        public ResultService(IResultRepository repo)
        {
            _repo = repo;
        }

        public void SaveResult(Result result)
        {
            result.DateTaken = DateTime.Now;
            _repo.Save(result);
        }

        public List<Result> GetUserResults(int userId) => _repo.GetByUser(userId);

        public List<Result> GetTopForQuiz(int quizId, int topN) => _repo.GetTopForQuiz(quizId, topN);
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using QuizApp.Models;

namespace QuizApp.Data
{
    public class SqlResultRepository : IResultRepository
    {
        private readonly string _conn;
        public SqlResultRepository(string connectionString)
        {
            _conn = connectionString;
        }

        public void Save(Result result)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                using (var cmd = new SqlCommand("INSERT INTO Results (UserId, QuizId, Score, DateTaken) VALUES (@u, @q, @s, @d)", conn))
                {
                    cmd.Parameters.AddWithValue("@u", result.UserId);
                    cmd.Parameters.AddWithValue("@q", result.QuizId);
                    cmd.Parameters.AddWithValue("@s", result.Score);
                    cmd.Parameters.AddWithValue("@d", result.DateTaken);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Result> GetByUser(int userId)
        {
            var list = new List<Result>();
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"SELECT r.ResultId, r.UserId, r.QuizId, r.Score, r.DateTaken, q.Title
                FROM Results r
                INNER JOIN Quizzes q ON r.QuizId = q.QuizId
                WHERE UserId = @u
                ORDER BY DateTaken DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@u", userId);
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new Result
                            {
                                ResultId = (int)r["ResultId"],
                                UserId = (int)r["UserId"],
                                QuizId = (int)r["QuizId"],
                                Score = (int)r["Score"],
                                DateTaken = (DateTime)r["DateTaken"],
                                QuizTitle = r["Title"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public List<Result> GetTopForQuiz(int quizId, int limit)
        {
            var results = new List<Result>();
            using (var conn = new SqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
            SELECT TOP(@limit) r.ResultId, r.UserId, r.QuizId, r.Score, r.DateTaken, u.Login
            FROM Results r
            INNER JOIN Users u ON r.UserId = u.UserId
            WHERE r.QuizId = @quizId
            ORDER BY r.Score DESC, r.DateTaken ASC", conn);

                cmd.Parameters.AddWithValue("@limit", limit);
                cmd.Parameters.AddWithValue("@quizId", quizId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        results.Add(new Result
                        {
                            ResultId = (int)r["ResultId"],
                            UserId = (int)r["UserId"],
                            QuizId = (int)r["QuizId"],
                            Score = (int)r["Score"],
                            DateTaken = (DateTime)r["DateTaken"],
                            UserLogin = r["Login"].ToString()
                        });
                    }
                }
            }
            return results;
        }
    }
}
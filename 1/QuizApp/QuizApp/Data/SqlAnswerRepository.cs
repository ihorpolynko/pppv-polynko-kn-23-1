using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using QuizApp.Models;

namespace QuizApp.Data
{
    public class SqlAnswerRepository : IAnswerRepository
    {
        private readonly string _conn;
        public SqlAnswerRepository(string connectionString) => _conn = connectionString;

        public List<Answer> GetByQuestion(int questionId)
        {
            var list = new List<Answer>();
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT AnswerId, QuestionId, Text, IsCorrect FROM Answers WHERE QuestionId=@id", conn);
                cmd.Parameters.AddWithValue("@id", questionId);
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new Answer
                        {
                            AnswerId = (int)r["AnswerId"],
                            QuestionId = (int)r["QuestionId"],
                            Text = (string)r["Text"],
                            IsCorrect = (bool)r["IsCorrect"]
                        });
                    }
                }
            }
            return list;
        }

        public void Add(Answer answer)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO Answers (QuestionId, Text, IsCorrect) VALUES (@qid, @text, @correct); SELECT SCOPE_IDENTITY();", conn);
                cmd.Parameters.AddWithValue("@qid", answer.QuestionId);
                cmd.Parameters.AddWithValue("@text", answer.Text);
                cmd.Parameters.AddWithValue("@correct", answer.IsCorrect);
                answer.AnswerId = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public void Update(Answer answer)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE Answers SET Text=@text, IsCorrect=@correct WHERE AnswerId=@id", conn);
                cmd.Parameters.AddWithValue("@text", answer.Text);
                cmd.Parameters.AddWithValue("@correct", answer.IsCorrect);
                cmd.Parameters.AddWithValue("@id", answer.AnswerId);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int answerId)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Answers WHERE AnswerId=@id", conn);
                cmd.Parameters.AddWithValue("@id", answerId);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteByQuestion(int questionId)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Answers WHERE QuestionId=@id", conn);
                cmd.Parameters.AddWithValue("@id", questionId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using QuizApp.Models;

namespace QuizApp.Data
{
    public class SqlQuestionRepository : IQuestionRepository
    {
        private readonly string _conn;
        public SqlQuestionRepository(string connectionString) => _conn = connectionString;

        public List<Question> GetByQuiz(int quizId)
        {
            var list = new List<Question>();
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT QuestionId, QuizId, Text FROM Questions WHERE QuizId=@id", conn);
                cmd.Parameters.AddWithValue("@id", quizId);
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new Question
                        {
                            QuestionId = (int)r["QuestionId"],
                            QuizId = (int)r["QuizId"],
                            Text = (string)r["Text"]
                        });
                    }
                }
            }
            return list;
        }

        public Question GetById(int questionId)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT QuestionId, QuizId, Text FROM Questions WHERE QuestionId=@id", conn);
                cmd.Parameters.AddWithValue("@id", questionId);
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                        return new Question
                        {
                            QuestionId = (int)r["QuestionId"],
                            QuizId = (int)r["QuizId"],
                            Text = (string)r["Text"]
                        };
                    return null;
                }
            }
        }

        public void Add(Question question)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO Questions (QuizId, Text) VALUES (@quizId, @text); SELECT SCOPE_IDENTITY();", conn);
                cmd.Parameters.AddWithValue("@quizId", question.QuizId);
                cmd.Parameters.AddWithValue("@text", question.Text);
                question.QuestionId = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


        public void Update(Question question)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE Questions SET Text=@text WHERE QuestionId=@id", conn);
                cmd.Parameters.AddWithValue("@text", question.Text);
                cmd.Parameters.AddWithValue("@id", question.QuestionId);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int questionId)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Questions WHERE QuestionId=@id", conn);
                cmd.Parameters.AddWithValue("@id", questionId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
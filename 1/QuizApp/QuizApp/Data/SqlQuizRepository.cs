using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using QuizApp.Models;

namespace QuizApp.Data
{
    public class SqlQuizRepository : IQuizRepository
    {
        private readonly string _conn;
        public SqlQuizRepository(string connectionString)
        {
            _conn = connectionString;
        }

        public Quiz GetById(int id)
        {
            var quiz = new Quiz();
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT QuizId, Title FROM Quizzes WHERE QuizId = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            quiz.QuizId = (int)r["QuizId"];
                            quiz.Title = (string)r["Title"];
                        }
                        else return null;
                    }
                }

                using (var qcmd = new SqlCommand("SELECT QuestionId, Text FROM Questions WHERE QuizId = @quizId", conn))
                {
                    qcmd.Parameters.AddWithValue("@quizId", id);
                    using (var r = qcmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            quiz.Questions.Add(new Question
                            {
                                QuestionId = (int)r["QuestionId"],
                                QuizId = id,
                                Text = (string)r["Text"]
                            });
                        }
                    }
                }

                foreach (var question in quiz.Questions)
                {
                    using (var acmd = new SqlCommand("SELECT AnswerId, Text, IsCorrect FROM Answers WHERE QuestionId = @qId", conn))
                    {
                        acmd.Parameters.AddWithValue("@qId", question.QuestionId);
                        using (var r = acmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                question.Answers.Add(new Answer
                                {
                                    AnswerId = (int)r["AnswerId"],
                                    QuestionId = question.QuestionId,
                                    Text = (string)r["Text"],
                                    IsCorrect = (bool)r["IsCorrect"]
                                });
                            }
                        }
                    }
                }
            }
            return quiz;
        }

        public List<Quiz> GetAll()
        {
            var res = new List<Quiz>();
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT QuizId, Title FROM Quizzes", conn))
                {
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            res.Add(new Quiz
                            {
                                QuizId = (int)r["QuizId"],
                                Title = (string)r["Title"]
                            });
                        }
                    }
                }
            }
            return res;
        }

        public void Delete(int quizId)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var cmd1 = new SqlCommand(
                            "DELETE A FROM Answers A INNER JOIN Questions Q ON A.QuestionId=Q.QuestionId WHERE Q.QuizId=@id",
                            conn, tran);
                        cmd1.Parameters.AddWithValue("@id", quizId);
                        cmd1.ExecuteNonQuery();

                        var cmd2 = new SqlCommand("DELETE FROM Questions WHERE QuizId=@id", conn, tran);
                        cmd2.Parameters.AddWithValue("@id", quizId);
                        cmd2.ExecuteNonQuery();

                        var cmd3 = new SqlCommand("DELETE FROM Quizzes WHERE QuizId=@id", conn, tran);
                        cmd3.Parameters.AddWithValue("@id", quizId);
                        cmd3.ExecuteNonQuery();

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Add(Quiz quiz)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                using (var cmd = new SqlCommand("INSERT INTO Quizzes (Title) VALUES (@title); SELECT CAST(SCOPE_IDENTITY() AS INT);", conn))
                {
                    cmd.Parameters.AddWithValue("@title", quiz.Title ?? "");

                    quiz.QuizId = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Quiz quiz)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                using (var cmd = new SqlCommand("UPDATE Quizzes SET Title=@title WHERE QuizId=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@title", quiz.Title ?? "");
                    cmd.Parameters.AddWithValue("@id", quiz.QuizId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
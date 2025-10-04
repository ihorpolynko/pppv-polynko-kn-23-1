using System;
using System.Data.SqlClient;
using QuizApp.Models;

namespace QuizApp.Data
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly string _conn;
        public SqlUserRepository(string connectionString)
        {
            _conn = connectionString;
        }

        public User GetByLogin(string login)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT UserId, Login, Password, BirthDate FROM Users WHERE Login = @login", conn))
                {
                    cmd.Parameters.AddWithValue("@login", login);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new User
                            {
                                UserId = (int)r["UserId"],
                                Login = (string)r["Login"],
                                Password = (string)r["Password"],
                                BirthDate = (DateTime)r["BirthDate"]
                            };
                        }
                    }
                }
            }
            return null;
        }

        public User GetById(int id)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT UserId, Login, Password, BirthDate FROM Users WHERE UserId = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new User
                            {
                                UserId = (int)r["UserId"],
                                Login = (string)r["Login"],
                                Password = (string)r["Password"],
                                BirthDate = (DateTime)r["BirthDate"]
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void Add(User user)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                using (var cmd = new SqlCommand("INSERT INTO Users (Login, Password, BirthDate) VALUES (@login, @pass, @birth)", conn))
                {
                    cmd.Parameters.AddWithValue("@login", user.Login);
                    cmd.Parameters.AddWithValue("@pass", user.Password);
                    cmd.Parameters.AddWithValue("@birth", user.BirthDate);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(User user)
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                UPDATE Users SET Password = @pass,BirthDate = @birth WHERE UserId = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@pass", user.Password);
                    cmd.Parameters.AddWithValue("@birth", user.BirthDate);
                    cmd.Parameters.AddWithValue("@id", user.UserId);
                    cmd.ExecuteNonQuery();
                }

            }
        }
    }
}
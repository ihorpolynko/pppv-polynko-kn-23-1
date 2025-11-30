using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Data
{
    public class SqlRepositoryFactory : RepositoryFactory
    {
        private readonly string _connectionString;

        public SqlRepositoryFactory()
        {
            _connectionString = DbConfig.Instance.ConnectionString;
        }

        public override IQuestionRepository CreateQuestionRepository()
        {
            return new SqlQuestionRepository(_connectionString);
        }

        public override IUserRepository CreateUserRepository()
        {
            return new SqlUserRepository(_connectionString);
        }

        public override IResultRepository CreateResultRepository()
        {
            return new SqlResultRepository(_connectionString);
        }

        public override IQuizRepository CreateQuizRepository()
        {
            return new SqlQuizRepository(_connectionString);
        }

        public override IAnswerRepository CreateAnswerRepository()
        {
            return new SqlAnswerRepository(_connectionString);
        }
    }
}
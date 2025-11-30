using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Data
{
    public abstract class RepositoryFactory
    {
        public abstract IQuestionRepository CreateQuestionRepository();
        public abstract IUserRepository CreateUserRepository();
        public abstract IResultRepository CreateResultRepository();
        public abstract IQuizRepository CreateQuizRepository();
        public abstract IAnswerRepository CreateAnswerRepository();

    }
}
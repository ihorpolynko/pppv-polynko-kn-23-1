using System.Collections.Generic;
using QuizApp.Models;

namespace QuizApp.Services
{
    public class TemplateQuizBuilder
    {
        private Quiz _quiz;

        public TemplateQuizBuilder()
        {
            _quiz = new Quiz
            {
                Questions = new List<Question>()
            };
        }

        public TemplateQuizBuilder SetTitle(string title)
        {
            _quiz.Title = title;
            _quiz.Category = "Шаблон";
            return this;
        }

        public TemplateQuizBuilder AddDefaultQuestion()
        {
            var question = new Question
            {
                Text = "Шаблонне питання",
                Answers = new List<Answer>
                {
                    new Answer { Text = "Шаблонна відповідь 1", IsCorrect = true },
                    new Answer { Text = "Шаблонна відповідь 2", IsCorrect = false },
                    new Answer { Text = "Шаблонна відповідь 3", IsCorrect = false }
                }
            };

            _quiz.Questions.Add(question);
            return this;
        }

        public Quiz Build()
        {
            return _quiz;
        }
    }
}
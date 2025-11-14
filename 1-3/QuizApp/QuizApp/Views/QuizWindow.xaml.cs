using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using QuizApp.Data;
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Views
{
    public partial class QuizWindow : Window
    {
        private readonly User _user;
        private readonly QuizService _quizService;
        private readonly ResultService _resultService;
        private List<Question> _questions = new List<Question>();
        private int _currentIndex = 0;
        private Dictionary<int, List<int>> _answersByQuestion = new Dictionary<int, List<int>>();
        private int _quizId;
        private bool _isMixed;

        public QuizWindow(User user, int quizId, bool isMixed)
        {
            InitializeComponent();
            _user = user;

            var factory = new SqlRepositoryFactory();

            var quizRepo = factory.CreateQuizRepository();
            var resultRepo = factory.CreateResultRepository();

            _quizService = new QuizService(quizRepo);
            _resultService = new ResultService(resultRepo);

            _quizId = quizId;
            _isMixed = isMixed;

            LoadQuestions();
            RenderQuestion();
        }

        private void LoadQuestions()
        {
            if (_isMixed)
            {
                _questions = _quizService.GetMixedQuestions(20);
                QuizTitle.Text = "Змішана вікторина — 20 питань";
            }
            else
            {
                var qz = _quizService.GetQuizWithQuestions(_quizId);
                QuizTitle.Text = qz?.Title ?? "Вікторина";
                _questions = _quizService.GetRandomQuestions(_quizId, 20);
            }
        }

        private void RenderQuestion()
        {
            QuestionArea.Children.Clear();

            if (_questions == null || !_questions.Any())
            {
                QuestionArea.Children.Add(new TextBlock { Text = "Питань нема.", FontWeight = FontWeights.Bold });
                return;
            }
            var q = _questions[_currentIndex];
            var tb = new TextBlock { Text = $"{_currentIndex + 1}. {q.Text}", TextWrapping = TextWrapping.Wrap, FontSize = 14, Margin = new Thickness(0, 0, 0, 8) };
            QuestionArea.Children.Add(tb);

            var answersPanel = new StackPanel();
            foreach (var a in q.Answers)
            {
                var cb = new CheckBox { Content = a.Text, Tag = a.AnswerId, Margin = new Thickness(0, 4, 0, 4) };
                if (_answersByQuestion.TryGetValue(q.QuestionId, out var list) && list.Contains(a.AnswerId)) cb.IsChecked = true;
                answersPanel.Children.Add(cb);
            }
            QuestionArea.Children.Add(answersPanel);

            var status = new TextBlock { Text = $"Питання {_currentIndex + 1} з {_questions.Count}", Margin = new Thickness(0, 8, 0, 0) };
            QuestionArea.Children.Add(status);
        }

        private void SaveCurrentSelections()
        {
            var q = _questions[_currentIndex];
            var answersPanel = QuestionArea.Children.OfType<StackPanel>().FirstOrDefault();
            if (answersPanel == null) return;
            var chosen = new List<int>();
            foreach (var cb in answersPanel.Children.OfType<CheckBox>())
            {
                if (cb.IsChecked == true) chosen.Add((int)cb.Tag);
            }
            _answersByQuestion[q.QuestionId] = chosen;
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentSelections();
            if (_currentIndex > 0)
            {
                _currentIndex--;
                RenderQuestion();
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentSelections();
            if (_currentIndex < _questions.Count - 1)
            {
                _currentIndex++;
                RenderQuestion();
            }
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentSelections();
            int correct = 0;
            foreach (var q in _questions)
            {
                _answersByQuestion.TryGetValue(q.QuestionId, out var chosen);
                if (_quizService.IsQuestionCorrect(q, chosen)) correct++;
            }
            MessageBox.Show($"Тест завершений. Правильних відповідей: {correct} з {_questions.Count}", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);

            var result = new Result
            {
                UserId = _user.UserId,
                QuizId = _isMixed ? 0 : _quizId,
                Score = correct
            };
            _resultService.SaveResult(result);
            this.Close();
        }
    }
}
using QuizApp.Data;
using QuizApp.Models;
using QuizApp.Services;
using System.Windows;

namespace QuizApp.Views
{
    public partial class QuestionEditorWindow : Window
    {
        private readonly int _quizId;
        private int? _questionId;
        private readonly QuestionService _questionService;
        private readonly AnswerService _answerService;

        public QuestionEditorWindow(int quizId, int? questionId)
        {
            InitializeComponent();
            _quizId = quizId;
            _questionId = questionId;

            var factory = new SqlRepositoryFactory();

            var questionRepo = factory.CreateQuestionRepository();
            var answerRepo = factory.CreateAnswerRepository();

            _answerService = new AnswerService(answerRepo);
            _questionService = new QuestionService(questionRepo, answerRepo);

            if (_questionId.HasValue)
                LoadQuestion(_questionId.Value);
        }

        private void LoadQuestion(int questionId)
        {
            var question = _questionService.GetById(questionId);
            if (question != null)
            {
                QuestionTextBox.Text = question.Text;
                AnswerListBox.ItemsSource = question.Answers;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_questionId.HasValue)
            {
                var question = _questionService.GetById(_questionId.Value);
                question.Text = QuestionTextBox.Text;
                _questionService.UpdateQuestion(question);
            }
            else
            {
                var question = new Question
                {
                    QuizId = _quizId,
                    Text = QuestionTextBox.Text
                };
                _questionService.AddQuestion(question);
            }

            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnAddAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (!_questionId.HasValue)
            {
                var newQuestion = new Question
                {
                    QuizId = _quizId,
                    Text = QuestionTextBox.Text
                };
                _questionService.AddQuestion(newQuestion);

                _questionId = newQuestion.QuestionId;
            }

            var editor = new AnswerEditorWindow();
            if (editor.ShowDialog() == true)
            {
                var answer = new Answer
                {
                    QuestionId = _questionId.Value,
                    Text = editor.AnswerText,
                    IsCorrect = editor.IsCorrect
                };

                _answerService.AddAnswer(answer);
                LoadQuestion(_questionId.Value);
            }
        }

        private void BtnEditAnswer_Click(object sender, RoutedEventArgs e)
        {
            var selected = AnswerListBox.SelectedItem as Answer;
            if (selected == null) return;

            var editor = new AnswerEditorWindow(selected.Text, selected.IsCorrect);
            if (editor.ShowDialog() == true)
            {
                selected.Text = editor.AnswerText;
                selected.IsCorrect = editor.IsCorrect;
                _answerService.UpdateAnswer(selected);
                LoadQuestion(_questionId.Value);
            }
        }

        private void BtnDeleteAnswer_Click(object sender, RoutedEventArgs e)
        {
            var selected = AnswerListBox.SelectedItem as Answer;
            if (selected == null) return;

            if (MessageBox.Show("Видалити відповідь?", "Підтвердження", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            _answerService.DeleteAnswer(selected.AnswerId);
            LoadQuestion(_questionId.Value);
        }
    }
}
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
    public partial class AdminWindow : Window
    {
        private readonly QuizService _quizService;
        private readonly QuestionService _questionService;
        private readonly AnswerService _answerService;

        private List<Quiz> _quizzes = new List<Quiz>();
        private List<Question> _questions = new List<Question>();
        private Quiz _selectedQuiz;
        private Question _selectedQuestion;

        public AdminWindow()
        {
            InitializeComponent();

            var factory = new SqlRepositoryFactory();

            var quizRepo = factory.CreateQuizRepository();
            var questionRepo = factory.CreateQuestionRepository();
            var answerRepo = factory.CreateAnswerRepository();

            _quizService = new QuizService(quizRepo);
            _answerService = new AnswerService(answerRepo);
            _questionService = new QuestionService(questionRepo, answerRepo);

            LoadQuizzes();
        }

        private void LoadQuizzes()
        {
            _quizzes = _quizService.GetAllQuizzes()
                .Where(q => q.QuizId != 0)
                .ToList();

            QuizListBox.ItemsSource = _quizzes;
        }

        private void BtnAddQuiz_Click(object sender, RoutedEventArgs e)
        {
            var editor = new QuizEditorWindow(new Quiz());
            if (editor.ShowDialog() == true)
            {
                _quizService.AddQuiz(editor.Quiz);
                LoadQuizzes();
            }
        }

        private void BtnCreateTemplateQuiz_Click(object sender, RoutedEventArgs e)
        {
            var builder = new TemplateQuizBuilder();
            var quiz = builder
                .SetTitle("Шаблонна вікторина")
                .AddDefaultQuestion()
                .Build();

            _quizService.AddQuiz(quiz);

            var savedQuiz = _quizService
                .GetAllQuizzes()
                .FirstOrDefault(q => q.Title == quiz.Title);

            if (savedQuiz != null && quiz.Questions.Count > 0)
            {
                foreach (var question in quiz.Questions)
                {
                    question.QuizId = savedQuiz.QuizId;
                    _questionService.AddQuestion(question);

                    foreach (var answer in question.Answers)
                    {
                        answer.QuestionId = question.QuestionId;
                        _answerService.AddAnswer(answer);
                    }
                }
            }

            LoadQuizzes();
            MessageBox.Show("Шаблонна вікторина створена!");
        }


        private void BtnEditQuiz_Click(object sender, RoutedEventArgs e)
        {
            _selectedQuiz = QuizListBox.SelectedItem as Quiz;
            if (_selectedQuiz == null)
            {
                MessageBox.Show("Виберіть вікторину для редагування.");
                return;
            }

            var editor = new QuizEditorWindow(_selectedQuiz);
            if (editor.ShowDialog() == true)
            {
                _quizService.UpdateQuiz(editor.Quiz);
                LoadQuizzes();
            }

        }

        private void BtnDeleteQuiz_Click(object sender, RoutedEventArgs e)
        {
            _selectedQuiz = QuizListBox.SelectedItem as Quiz;
            if (_selectedQuiz == null) return;

            if (MessageBox.Show($"Видалити вікторину '{_selectedQuiz.Title}' і всі її питання?", "Підтвердження", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            _quizService.DeleteQuiz(_selectedQuiz.QuizId);
            LoadQuizzes();
            QuestionListBox.ItemsSource = null;
        }

        private void QuizListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedQuiz = QuizListBox.SelectedItem as Quiz;
            if (_selectedQuiz == null)
            {
                QuestionListBox.ItemsSource = null;
                return;
            }

            LoadQuestions(_selectedQuiz.QuizId);
        }

        private void LoadQuestions(int quizId)
        {
            _questions = _questionService.GetQuestionsByQuiz(quizId);
            QuestionListBox.ItemsSource = _questions;
        }

        private void BtnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedQuiz == null) { MessageBox.Show("Спершу виберіть викторину."); return; }

            var editor = new QuestionEditorWindow(_selectedQuiz.QuizId, null);
            if (editor.ShowDialog() == true)
            {
                LoadQuestions(_selectedQuiz.QuizId);
            }
        }

        private void BtnEditQuestion_Click(object sender, RoutedEventArgs e)
        {
            _selectedQuestion = QuestionListBox.SelectedItem as Question;
            if (_selectedQuestion == null) { MessageBox.Show("Виберіть питання для редагування."); return; }

            var editor = new QuestionEditorWindow(_selectedQuiz.QuizId, _selectedQuestion.QuestionId);
            if (editor.ShowDialog() == true)
            {
                LoadQuestions(_selectedQuiz.QuizId);
            }
        }

        private void BtnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            _selectedQuestion = QuestionListBox.SelectedItem as Question;
            if (_selectedQuestion == null) return;

            if (MessageBox.Show($"Видалити питання '{_selectedQuestion.Text}' і всі його відповіді?", "Підтвердження", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            _questionService.DeleteQuestion(_selectedQuestion.QuestionId);
            LoadQuestions(_selectedQuiz.QuizId);
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            var loginWin = new LoginWindow();
            loginWin.Show();
        }
    }
}
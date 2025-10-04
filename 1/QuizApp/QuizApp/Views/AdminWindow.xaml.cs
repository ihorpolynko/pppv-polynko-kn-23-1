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

            var quizRepo = new SqlQuizRepository(DbConfig.ConnectionString);
            var questionRepo = new SqlQuestionRepository(DbConfig.ConnectionString);
            var answerRepo = new SqlAnswerRepository(DbConfig.ConnectionString);

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
            var editor = new QuizEditorWindow();
            if (editor.ShowDialog() == true)
            {
                var quiz = new Quiz { Title = editor.QuizTitle };
                _quizService.AddQuiz(quiz);
                LoadQuizzes();
            }
        }

        private void BtnEditQuiz_Click(object sender, RoutedEventArgs e)
        {
            _selectedQuiz = QuizListBox.SelectedItem as Quiz;
            if (_selectedQuiz == null)
            {
                MessageBox.Show("Виберіть вікторину для редагування.");
                return;
            }

            var editor = new QuizEditorWindow(_selectedQuiz.Title);
            if (editor.ShowDialog() == true)
            {
                _selectedQuiz.Title = editor.QuizTitle;
                _quizService.UpdateQuiz(_selectedQuiz);
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
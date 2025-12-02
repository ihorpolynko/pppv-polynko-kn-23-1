using System.Linq;
using System.Windows;
using QuizApp.Data;
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly User _user;
        private readonly QuizService _quizService;
        private readonly ResultService _resultService;

        public MainWindow(User user)
        {
            InitializeComponent();
            _user = user;
            WelcomeText.Text = $"Привіт, {_user.Login}!";

            var factory = new SqlRepositoryFactory();

            var quizRepo = factory.CreateQuizRepository();
            var resultRepo = factory.CreateResultRepository();

            _quizService = new QuizService(quizRepo);
            _resultService = new ResultService(resultRepo);

            LoadQuizzes();
        }

        private void LoadQuizzes()
        {
            var list = _quizService.GetAllQuizzes();
            QuizListBox.ItemsSource = list;
        }

        private void BtnStartQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (MixedQuizCheck.IsChecked == true)
            {
                var quizWin = new QuizWindow(_user, 0, true);
                quizWin.ShowDialog();
            }
            else
            {
                MessageBox.Show("Виберіть вікторину і натисніть 'Вибрати і старт' або вмикніть 'Змішана'.");
            }
        }

        private void BtnOpenQuiz_Click(object sender, RoutedEventArgs e)
        {
            var selected = QuizListBox.SelectedItem as Quiz;
            if (selected == null)
            {
                MessageBox.Show("Виберіть вікторину.");
                return;
            }
            var quizWin = new QuizWindow(_user, selected.QuizId, false);
            quizWin.ShowDialog();
        }

        private void BtnResults_Click(object sender, RoutedEventArgs e)
        {
            var w = new ResultsWindow(_user);
            w.ShowDialog();
        }

        private void BtnTop20_Click(object sender, RoutedEventArgs e)
        {
            var selected = QuizListBox.SelectedItem as Quiz;
            if (selected == null)
            {
                MessageBox.Show("Виберіть вікторину для перегляду Топ-20.");
                return;
            }

            var topList = _resultService.GetTopForQuiz(selected.QuizId, 20);
            string s = $"Топ-20 — {selected.Title}\n\n";
            int pos = 1;
            foreach (var r in topList)
            {
                s += $"№{pos} \t {r.UserLogin} \t {r.Score} правильних відповідей \t {r.DateTaken}\n";
                pos++;
            }
            MessageBox.Show(s, "Топ-20");
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            var w = new SettingsWindow(_user);
            w.Owner = this;
            w.ShowDialog();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
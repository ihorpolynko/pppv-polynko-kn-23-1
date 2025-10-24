using System.Windows;
using QuizApp.Data;
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Views
{
    public partial class ResultsWindow : Window
    {
        private readonly User _user;
        private readonly ResultService _resultService;

        public ResultsWindow(User user)
        {
            InitializeComponent();
            _user = user;

            var factory = new SqlRepositoryFactory();

            var resultRepo = factory.CreateResultRepository();

            _resultService = new ResultService(resultRepo);
            
            Load();

            this.DataContext = user;
        }

        private void Load()
        {
            var list = _resultService.GetUserResults(_user.UserId);
            ResultsList.ItemsSource = list;
        }
    }
}
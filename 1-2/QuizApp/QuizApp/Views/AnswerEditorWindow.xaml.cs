using System.Windows;

namespace QuizApp.Views
{
    public partial class AnswerEditorWindow : Window
    {
        public string AnswerText { get; private set; }
        public bool IsCorrect { get; private set; }

        public AnswerEditorWindow(string text = "", bool isCorrect = false)
        {
            InitializeComponent();
            AnswerTextBox.Text = text;
            IsCorrectCheckBox.IsChecked = isCorrect;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            AnswerText = AnswerTextBox.Text;
            IsCorrect = IsCorrectCheckBox.IsChecked ?? false;
            DialogResult = true;
        }
    }
}

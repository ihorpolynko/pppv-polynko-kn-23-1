using System.Windows;

namespace QuizApp.Views
{
    public partial class QuizEditorWindow : Window
    {
        public string QuizTitle { get; private set; }

        public QuizEditorWindow(string defaultTitle = "")
        {
            InitializeComponent();
            TitleTextBox.Text = defaultTitle;
            TitleTextBox.Focus();
            TitleTextBox.SelectAll();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Введіть назву вікторини.");
                return;
            }

            QuizTitle = TitleTextBox.Text.Trim();
            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
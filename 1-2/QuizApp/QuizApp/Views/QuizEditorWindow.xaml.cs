using QuizApp.Models;
using System.Windows;
using System.Windows.Controls;

namespace QuizApp.Views
{
    public partial class QuizEditorWindow : Window
    {
        public Quiz Quiz { get; private set; }

        public QuizEditorWindow(Quiz quiz)
        {
            InitializeComponent();

            Quiz = quiz ?? new Quiz();

            TitleTextBox.Text = Quiz.Title ?? "";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Введіть назву вікторини.");
                return;
            }

            Quiz.Title = TitleTextBox.Text.Trim();

            if (CategoryComboBox.SelectedItem is ComboBoxItem selectedItem)
                Quiz.Category = selectedItem.Content.ToString();
            else
                Quiz.Category = "Розваги";

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
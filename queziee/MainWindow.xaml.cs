using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using queziee.Models;
using queziee.Services;
using queziee.Views;

namespace queziee
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly QuizDataService _dataService;
        private List<Quiz> _quizzes = new List<Quiz>();
        private Quiz? _selectedQuiz;
        private GameWindow? _gameWindow;
        private OperatorControlWindow? _operatorWindow;
        
        public MainWindow()
        {
            InitializeComponent();
            _dataService = new QuizDataService();
            LoadQuizzes();
        }
        
        private async void LoadQuizzes()
        {
            try
            {
                _quizzes = await _dataService.GetQuizzesAsync();
                QuizComboBox.ItemsSource = _quizzes;
                if (_quizzes.Any())
                {
                    QuizComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading quizzes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void QuizComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedQuiz = QuizComboBox.SelectedItem as Quiz;
            UpdateQuizInfo();
        }
        
        private void UpdateQuizInfo()
        {
            if (_selectedQuiz == null)
            {
                QuizNameText.Text = "Geen quiz geselecteerd";
                QuizDescriptionText.Text = "";
                QuestionCountText.Text = "";
                TimePerQuestionText.Text = "";
                QuestionsListView.ItemsSource = null;
                return;
            }
            
            QuizNameText.Text = _selectedQuiz.Name;
            QuizDescriptionText.Text = _selectedQuiz.Description ?? "";
            QuestionCountText.Text = $"Aantal vragen: {_selectedQuiz.Questions.Count}";
            TimePerQuestionText.Text = $"Tijd per vraag: {_selectedQuiz.TimePerQuestion} seconden";
            QuestionsListView.ItemsSource = _selectedQuiz.Questions;
        }
        
        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedQuiz == null)
            {
                MessageBox.Show("Selecteer eerst een quiz!", "Geen Quiz", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (_selectedQuiz.Questions.Count < 1)
            {
                MessageBox.Show("De geselecteerde quiz heeft geen vragen!", "Geen Vragen", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            // Close existing windows if any
            if (_gameWindow != null)
            {
                _gameWindow.Close();
            }
            if (_operatorWindow != null)
            {
                _operatorWindow.Close();
            }
            
            // Create game window (always in quiz mode)
            _gameWindow = new GameWindow(_selectedQuiz);
            _gameWindow.Show();
            _gameWindow.WindowState = WindowState.Maximized;
            
            // Always open operator control window
            _operatorWindow = new OperatorControlWindow(_selectedQuiz, _gameWindow);
            _operatorWindow.Show();
            
            // When operator window closes, close game window too
            _operatorWindow.Closed += (s, args) =>
            {
                if (_gameWindow != null && _gameWindow.IsLoaded)
                {
                    _gameWindow.Close();
                }
            };
        }
        
        private void ManageQuestionsButton_Click(object sender, RoutedEventArgs e)
        {
            var questionManager = new QuestionManagerWindow(_dataService);
            questionManager.Owner = this;
            questionManager.ShowDialog();
            
            // Refresh quizzes after managing questions
            LoadQuizzes();
        }
        
        private void OpenGameScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (_gameWindow != null && _gameWindow.IsLoaded)
            {
                _gameWindow.Activate();
                _gameWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                MessageBox.Show("Er is geen actief speelscherm. Start eerst een quiz!", "Geen Speelscherm", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
            // Also bring operator window to front if it exists
            if (_operatorWindow != null && _operatorWindow.IsLoaded)
            {
                _operatorWindow.Activate();
            }
        }
    }
}
using System.Windows;
using System.Windows.Media;
using queziee.Models;

namespace queziee.Views
{
    public partial class OperatorControlWindow : Window
    {
        private readonly Quiz _quiz;
        private readonly GameWindow _gameWindow;
        private int _currentQuestionIndex = 0;
        private bool _answerRevealed = false;
        
        public OperatorControlWindow(Quiz quiz, GameWindow gameWindow)
        {
            InitializeComponent();
            _quiz = quiz;
            _gameWindow = gameWindow;
            
            // Subscribe to timer events from GameWindow
            _gameWindow.TimerTicked += OnGameWindowTimerTicked;
            _gameWindow.TimerFinished += OnGameWindowTimerFinished;
            
            InitializeOperatorPanel();
        }
        
        private void OnGameWindowTimerTicked(object? sender, int timeRemaining)
        {
            // Update timer display in operator panel
            TimerDisplay.Text = $"{timeRemaining}s";
            
            // Change color based on time remaining
            if (timeRemaining <= 5)
            {
                TimerDisplay.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38)); // Red
            }
            else if (timeRemaining <= 10)
            {
                TimerDisplay.Foreground = new SolidColorBrush(Color.FromRgb(251, 191, 36)); // Yellow
            }
            else
            {
                TimerDisplay.Foreground = new SolidColorBrush(Color.FromRgb(251, 191, 36)); // Yellow
            }
        }
        
        private void OnGameWindowTimerFinished(object? sender, EventArgs e)
        {
            // Show "TIJD OM!" in operator panel too
            TimerDisplay.Text = "TIJD OM!";
            TimerDisplay.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38)); // Red
        }
        
        private void InitializeOperatorPanel()
        {
            QuizNameText.Text = _quiz.Name;
            TimerDisplay.Text = $"{_quiz.TimePerQuestion}s";
            DisplayCurrentQuestion();
        }
        
        private void DisplayCurrentQuestion()
        {
            if (_currentQuestionIndex < 0 || _currentQuestionIndex >= _quiz.Questions.Count)
                return;
                
            var question = _quiz.Questions[_currentQuestionIndex];
            _answerRevealed = false;
            
            // Update question info
            QuestionNumberText.Text = $"Vraag {_currentQuestionIndex + 1} van {_quiz.Questions.Count}";
            CurrentQuestionText.Text = question.Text;
            
            // Update answers
            AnswerAText.Text = question.AnswerA;
            AnswerBText.Text = question.AnswerB;
            AnswerCText.Text = question.AnswerC;
            AnswerDText.Text = question.AnswerD;
            
            // Show correct answer
            string correctAnswerLetter = question.CorrectAnswer.ToString();
            string correctAnswerText = question.CorrectAnswer switch
            {
                'A' => question.AnswerA,
                'B' => question.AnswerB,
                'C' => question.AnswerC,
                'D' => question.AnswerD,
                _ => ""
            };
            CorrectAnswerText.Text = $"{correctAnswerLetter}: {correctAnswerText}";
            
            // Reset answer borders
            ResetAnswerBorders();
            
            // Reset timer display
            TimerDisplay.Text = $"{_quiz.TimePerQuestion}s";
            TimerDisplay.Foreground = new SolidColorBrush(Color.FromRgb(251, 191, 36)); // Yellow
            
            // Update buttons
            PreviousButton.IsEnabled = _currentQuestionIndex > 0;
            ShowAnswerButton.Visibility = Visibility.Visible;
            NextButton.Visibility = Visibility.Collapsed;
        }
        
        private void ResetAnswerBorders()
        {
            AnswerABorder.Background = new SolidColorBrush(Color.FromRgb(220, 38, 38)); // Red
            AnswerBBorder.Background = new SolidColorBrush(Color.FromRgb(37, 99, 235)); // Blue
            AnswerCBorder.Background = new SolidColorBrush(Color.FromRgb(202, 138, 4)); // Yellow
            AnswerDBorder.Background = new SolidColorBrush(Color.FromRgb(22, 163, 74)); // Green
        }
        
        private void HighlightCorrectAnswer()
        {
            if (_currentQuestionIndex < 0 || _currentQuestionIndex >= _quiz.Questions.Count)
                return;
                
            var question = _quiz.Questions[_currentQuestionIndex];
            var correctBrush = new SolidColorBrush(Color.FromRgb(16, 185, 129)); // Bright green
            var incorrectBrush = new SolidColorBrush(Color.FromRgb(75, 85, 99)); // Gray
            
            // Dim all answers first
            AnswerABorder.Background = incorrectBrush;
            AnswerBBorder.Background = incorrectBrush;
            AnswerCBorder.Background = incorrectBrush;
            AnswerDBorder.Background = incorrectBrush;
            
            // Highlight correct answer
            switch (question.CorrectAnswer)
            {
                case 'A':
                    AnswerABorder.Background = correctBrush;
                    break;
                case 'B':
                    AnswerBBorder.Background = correctBrush;
                    break;
                case 'C':
                    AnswerCBorder.Background = correctBrush;
                    break;
                case 'D':
                    AnswerDBorder.Background = correctBrush;
                    break;
            }
        }
        
        private void ShowAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_answerRevealed)
            {
                // Highlight correct answer in operator panel
                HighlightCorrectAnswer();
                
                // Tell the game window to show the answer
                _gameWindow.ShowCorrectAnswer();
                
                _answerRevealed = true;
                ShowAnswerButton.Visibility = Visibility.Collapsed;
                NextButton.Visibility = Visibility.Visible;
            }
        }
        
        private void StartTimerButton_Click(object sender, RoutedEventArgs e)
        {
            _gameWindow.StartTimer();
        }
        
        private void StopTimerButton_Click(object sender, RoutedEventArgs e)
        {
            _gameWindow.StopTimer();
        }
        
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuestionIndex < _quiz.Questions.Count - 1)
            {
                _currentQuestionIndex++;
                
                // Tell game window to move to next question
                _gameWindow.MoveToNextQuestion();
                
                DisplayCurrentQuestion();
            }
            else
            {
                // End of quiz
                var result = MessageBox.Show("Dit was de laatste vraag! Quiz beëindigen?", 
                                           "Quiz Voltooid", 
                                           MessageBoxButton.YesNo, 
                                           MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    _gameWindow.Close();
                    this.Close();
                }
            }
        }
        
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuestionIndex > 0)
            {
                _currentQuestionIndex--;
                
                // Tell game window to move to previous question
                _gameWindow.MoveToPreviousQuestion();
                
                DisplayCurrentQuestion();
            }
        }
        
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Weet je zeker dat je het operator panel wilt sluiten?", 
                                       "Bevestigen", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
        
        protected override void OnClosed(EventArgs e)
        {
            // Unsubscribe from events to prevent memory leaks
            _gameWindow.TimerTicked -= OnGameWindowTimerTicked;
            _gameWindow.TimerFinished -= OnGameWindowTimerFinished;
            
            base.OnClosed(e);
        }
    }
}

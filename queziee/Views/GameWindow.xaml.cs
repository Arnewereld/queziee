using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using queziee.Models;

namespace queziee.Views
{
    public partial class GameWindow : Window
    {
        private readonly Quiz _quiz;
        private int _currentQuestionIndex = 0;
        private DispatcherTimer? _timer;
        private int _timeRemaining;
        
        // Event to notify when timer updates
        public event EventHandler<int>? TimerTicked;
        public event EventHandler? TimerFinished;
        
        public GameWindow(Quiz quiz)
        {
            InitializeComponent();
            _quiz = quiz;
            
            InitializeGame();
        }
        
        private void InitializeGame()
        {
            QuizNameHeader.Text = _quiz.Name;
            ModeText.Text = "QUIZ MODUS";
            
            // Hide navigation buttons (controlled by operator panel)
            PreviousButton.Visibility = Visibility.Collapsed;
            NextButton.Visibility = Visibility.Collapsed;
            
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            
            DisplayCurrentQuestion();
        }
        
        private void DisplayCurrentQuestion()
        {
            if (_currentQuestionIndex < 0 || _currentQuestionIndex >= _quiz.Questions.Count)
                return;
                
            var question = _quiz.Questions[_currentQuestionIndex];
            
            QuestionText.Text = question.Text;
            QuestionNumberText.Text = $"Vraag {_currentQuestionIndex + 1} van {_quiz.Questions.Count}";
            
            // Set answer texts
            AnswerAText.Text = question.AnswerA ?? "";
            AnswerBText.Text = question.AnswerB ?? "";
            AnswerCText.Text = question.AnswerC ?? "";
            AnswerDText.Text = question.AnswerD ?? "";
            
            // Reset answer colors
            ResetAnswerColors();
            
            // Handle image if present
            if (!string.IsNullOrEmpty(question.ImagePath) && File.Exists(question.ImagePath))
            {
                QuestionImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(question.ImagePath));
                QuestionImage.Visibility = Visibility.Visible;
            }
            else
            {
                QuestionImage.Visibility = Visibility.Collapsed;
            }
            
            // Reset timer
            _timeRemaining = _quiz.TimePerQuestion;
            TimerText.Text = _timeRemaining.ToString();
            TimerText.Foreground = new SolidColorBrush(Color.FromRgb(251, 191, 36)); // Yellow
            _timer?.Stop();
            
            // Notify operator panel about reset
            TimerTicked?.Invoke(this, _timeRemaining);
        }
        
        private void ResetAnswerColors()
        {
            AnswerABorder.Background = new SolidColorBrush(Color.FromRgb(239, 68, 68)); // Red
            AnswerBBorder.Background = new SolidColorBrush(Color.FromRgb(59, 130, 246)); // Blue
            AnswerCBorder.Background = new SolidColorBrush(Color.FromRgb(234, 179, 8)); // Yellow
            AnswerDBorder.Background = new SolidColorBrush(Color.FromRgb(34, 197, 94)); // Green
        }
        
        private void HighlightCorrectAnswer()
        {
            if (_currentQuestionIndex < 0 || _currentQuestionIndex >= _quiz.Questions.Count)
                return;
                
            var question = _quiz.Questions[_currentQuestionIndex];
            var correctBrush = new SolidColorBrush(Color.FromRgb(16, 185, 129)); // Bright green
            var incorrectBrush = new SolidColorBrush(Color.FromRgb(107, 114, 128)); // Gray
            
            // Reset all to gray first
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
        
        // Public methods for operator control
        public void ShowCorrectAnswer()
        {
            HighlightCorrectAnswer();
        }
        
        public void MoveToNextQuestion()
        {
            if (_currentQuestionIndex < _quiz.Questions.Count - 1)
            {
                _currentQuestionIndex++;
                DisplayCurrentQuestion();
            }
        }
        
        public void MoveToPreviousQuestion()
        {
            if (_currentQuestionIndex > 0)
            {
                _currentQuestionIndex--;
                DisplayCurrentQuestion();
            }
        }
        
        public void StartTimer()
        {
            _timeRemaining = _quiz.TimePerQuestion;
            TimerText.Text = _timeRemaining.ToString();
            TimerText.Foreground = new SolidColorBrush(Color.FromRgb(251, 191, 36)); // Yellow
            _timer?.Start();
            
            // Notify operator panel
            TimerTicked?.Invoke(this, _timeRemaining);
        }
        
        public void StopTimer()
        {
            _timer?.Stop();
        }
        
        private void Timer_Tick(object? sender, EventArgs e)
        {
            _timeRemaining--;
            TimerText.Text = _timeRemaining.ToString();
            
            // Notify operator panel about each tick
            TimerTicked?.Invoke(this, _timeRemaining);
            
            if (_timeRemaining <= 0)
            {
                _timer?.Stop();
                TimerText.Text = "TIJD OM!";
                TimerText.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38)); // Red
                
                // Notify that timer finished
                TimerFinished?.Invoke(this, EventArgs.Empty);
            }
            else if (_timeRemaining <= 5)
            {
                TimerText.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38)); // Red
            }
            else if (_timeRemaining <= 10)
            {
                TimerText.Foreground = new SolidColorBrush(Color.FromRgb(251, 191, 36)); // Yellow
            }
            else
            {
                TimerText.Foreground = new SolidColorBrush(Color.FromRgb(251, 191, 36)); // Yellow
            }
        }
        
        private void StartTimerButton_Click(object sender, RoutedEventArgs e)
        {
            StartTimer();
        }
        
        private void StopTimerButton_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
        }
        
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            // Not used - controlled by operator panel
        }
        
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Not used - controlled by operator panel
        }
        
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();
            this.Close();
        }
        
        private void GameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Keyboard shortcuts removed - controlled by operator panel
        }
        
        protected override void OnClosed(EventArgs e)
        {
            _timer?.Stop();
            base.OnClosed(e);
        }
    }
}
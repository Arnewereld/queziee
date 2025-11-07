using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using queziee.Models;
using queziee.Services;

namespace queziee.Views
{
    public partial class QuestionManagerWindow : Window
    {
        private readonly QuizDataService _dataService;
        private List<Quiz> _quizzes = new List<Quiz>();
        private Quiz? _selectedQuiz;
        private Question? _selectedQuestion;
        private bool _isEditingNewQuiz = false;
        private bool _isEditingNewQuestion = false;
        private int _currentQuestionIndex = -1;
        
        public QuestionManagerWindow(QuizDataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;
            LoadQuizzes();
        }
        
        private async void LoadQuizzes()
        {
            try
            {
                _quizzes = await _dataService.GetQuizzesAsync();
                QuizListBox.ItemsSource = _quizzes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading quizzes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void QuizListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedQuiz = QuizListBox.SelectedItem as Quiz;
            _isEditingNewQuiz = false;
            
            if (_selectedQuiz != null)
            {
                QuestionsListBox.ItemsSource = _selectedQuiz.Questions;
                ShowQuizEditor();
            }
            else
            {
                QuestionsListBox.ItemsSource = null;
                HideAllEditors();
            }
            
            _selectedQuestion = null;
        }
        
        private void QuestionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedQuestion = QuestionsListBox.SelectedItem as Question;
            _isEditingNewQuestion = false;
            
            if (_selectedQuestion != null)
            {
                _currentQuestionIndex = _selectedQuiz?.Questions.IndexOf(_selectedQuestion) ?? -1;
                ShowQuestionEditor();
                UpdateQuestionNavigationButtons();
            }
        }
        
        private void ShowQuizEditor()
        {
            if (_selectedQuiz == null) return;
            
            QuizEditorPanel.Visibility = Visibility.Visible;
            QuestionEditPanel.Visibility = Visibility.Collapsed;
            DefaultMessageText.Visibility = Visibility.Collapsed;
            
            QuizNameTextBox.Text = _selectedQuiz.Name;
            QuizDescriptionTextBox.Text = _selectedQuiz.Description ?? "";
            TimePerQuestionTextBox.Text = _selectedQuiz.TimePerQuestion.ToString();
        }
        
        private void ShowQuestionEditor()
        {
            if (_selectedQuestion == null) return;
            
            QuizEditorPanel.Visibility = Visibility.Collapsed;
            QuestionEditPanel.Visibility = Visibility.Visible;
            DefaultMessageText.Visibility = Visibility.Collapsed;
            
            QuestionTextBox.Text = _selectedQuestion.Text;
            ImagePathTextBox.Text = _selectedQuestion.ImagePath ?? "";
            AnswerATextBox.Text = _selectedQuestion.AnswerA;
            AnswerBTextBox.Text = _selectedQuestion.AnswerB;
            AnswerCTextBox.Text = _selectedQuestion.AnswerC;
            AnswerDTextBox.Text = _selectedQuestion.AnswerD;
            
            // Set correct answer radio button
            CorrectAnswerA.IsChecked = _selectedQuestion.CorrectAnswer == 'A';
            CorrectAnswerB.IsChecked = _selectedQuestion.CorrectAnswer == 'B';
            CorrectAnswerC.IsChecked = _selectedQuestion.CorrectAnswer == 'C';
            CorrectAnswerD.IsChecked = _selectedQuestion.CorrectAnswer == 'D';
            
            UpdateQuestionNavigationButtons();
        }
        
        private void UpdateQuestionNavigationButtons()
        {
            if (_selectedQuiz == null || _currentQuestionIndex < 0)
            {
                PreviousQuestionButton.IsEnabled = false;
                NextQuestionButton.IsEnabled = false;
                return;
            }
            
            PreviousQuestionButton.IsEnabled = _currentQuestionIndex > 0;
            NextQuestionButton.IsEnabled = _currentQuestionIndex < _selectedQuiz.Questions.Count - 1;
        }
        
        private void PreviousQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedQuiz == null || _currentQuestionIndex <= 0) return;
            
            _currentQuestionIndex--;
            _selectedQuestion = _selectedQuiz.Questions[_currentQuestionIndex];
            QuestionsListBox.SelectedIndex = _currentQuestionIndex;
            ShowQuestionEditor();
        }
        
        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedQuiz == null || _currentQuestionIndex >= _selectedQuiz.Questions.Count - 1) return;
            
            _currentQuestionIndex++;
            _selectedQuestion = _selectedQuiz.Questions[_currentQuestionIndex];
            QuestionsListBox.SelectedIndex = _currentQuestionIndex;
            ShowQuestionEditor();
        }
        
        private void HideAllEditors()
        {
            QuizEditorPanel.Visibility = Visibility.Collapsed;
            QuestionEditPanel.Visibility = Visibility.Collapsed;
            DefaultMessageText.Visibility = Visibility.Visible;
        }
        
        private void NewQuizButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedQuiz = new Quiz
            {
                Name = "Nieuwe Quiz",
                Description = "",
                TimePerQuestion = 30,
                Questions = new List<Question>()
            };
            _isEditingNewQuiz = true;
            
            ShowQuizEditor();
            QuizNameTextBox.Focus();
            QuizNameTextBox.SelectAll();
        }
        
        private async void SaveQuizButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedQuiz == null) return;
            
            if (string.IsNullOrWhiteSpace(QuizNameTextBox.Text))
            {
                MessageBox.Show("Quiz naam is verplicht!", "Validatie Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (!int.TryParse(TimePerQuestionTextBox.Text, out int timePerQuestion) || timePerQuestion <= 0)
            {
                MessageBox.Show("Tijd per vraag moet een positief getal zijn!", "Validatie Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            _selectedQuiz.Name = QuizNameTextBox.Text.Trim();
            _selectedQuiz.Description = QuizDescriptionTextBox.Text.Trim();
            _selectedQuiz.TimePerQuestion = timePerQuestion;
            
            try
            {
                if (_isEditingNewQuiz)
                {
                    await _dataService.AddQuizAsync(_selectedQuiz);
                    _isEditingNewQuiz = false;
                }
                else
                {
                    await _dataService.UpdateQuizAsync(_selectedQuiz);
                }
                
                LoadQuizzes();
                MessageBox.Show("Quiz opgeslagen!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving quiz: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async void DeleteQuizButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedQuiz == null) return;
            
            var result = MessageBox.Show($"Weet je zeker dat je de quiz '{_selectedQuiz.Name}' wilt verwijderen?", 
                                       "Bevestigen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _dataService.DeleteQuizAsync(_selectedQuiz.Id);
                    LoadQuizzes();
                    HideAllEditors();
                    MessageBox.Show("Quiz verwijderd!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting quiz: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        private void NewQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedQuiz == null)
            {
                MessageBox.Show("Selecteer eerst een quiz!", "Geen Quiz", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            _selectedQuestion = new Question
            {
                Text = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = 'A',
                QuizId = _selectedQuiz.Id
            };
            _isEditingNewQuestion = true;
            
            ShowQuestionEditor();
            QuestionTextBox.Focus();
        }
        
        private async void SaveQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedQuestion == null || _selectedQuiz == null) return;
            
            if (string.IsNullOrWhiteSpace(QuestionTextBox.Text) ||
                string.IsNullOrWhiteSpace(AnswerATextBox.Text) ||
                string.IsNullOrWhiteSpace(AnswerBTextBox.Text) ||
                string.IsNullOrWhiteSpace(AnswerCTextBox.Text) ||
                string.IsNullOrWhiteSpace(AnswerDTextBox.Text))
            {
                MessageBox.Show("Alle velden zijn verplicht!", "Validatie Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            char correctAnswer = 'A';
            if (CorrectAnswerB.IsChecked == true) correctAnswer = 'B';
            else if (CorrectAnswerC.IsChecked == true) correctAnswer = 'C';
            else if (CorrectAnswerD.IsChecked == true) correctAnswer = 'D';
            
            _selectedQuestion.Text = QuestionTextBox.Text.Trim();
            _selectedQuestion.ImagePath = string.IsNullOrWhiteSpace(ImagePathTextBox.Text) ? null : ImagePathTextBox.Text.Trim();
            _selectedQuestion.AnswerA = AnswerATextBox.Text.Trim();
            _selectedQuestion.AnswerB = AnswerBTextBox.Text.Trim();
            _selectedQuestion.AnswerC = AnswerCTextBox.Text.Trim();
            _selectedQuestion.AnswerD = AnswerDTextBox.Text.Trim();
            _selectedQuestion.CorrectAnswer = correctAnswer;
            
            try
            {
                if (_isEditingNewQuestion)
                {
                    var maxId = _selectedQuiz.Questions.Any() ? _selectedQuiz.Questions.Max(q => q.Id) : 0;
                    _selectedQuestion.Id = maxId + 1;
                    _selectedQuestion.QuizId = _selectedQuiz.Id;
                    _selectedQuiz.Questions.Add(_selectedQuestion);
                    _isEditingNewQuestion = false;
                }
                
                await _dataService.UpdateQuizAsync(_selectedQuiz);
                
                // Refresh the questions list
                QuestionsListBox.ItemsSource = null;
                QuestionsListBox.ItemsSource = _selectedQuiz.Questions;
                
                MessageBox.Show("Vraag opgeslagen!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving question: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async void DeleteQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedQuestion == null || _selectedQuiz == null) return;
            
            var result = MessageBox.Show("Weet je zeker dat je deze vraag wilt verwijderen?", 
                                       "Bevestigen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _selectedQuiz.Questions.Remove(_selectedQuestion);
                    await _dataService.UpdateQuizAsync(_selectedQuiz);
                    
                    // Refresh the questions list
                    QuestionsListBox.ItemsSource = null;
                    QuestionsListBox.ItemsSource = _selectedQuiz.Questions;
                    
                    HideAllEditors();
                    MessageBox.Show("Vraag verwijderd!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting question: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        private void BrowseImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Selecteer een afbeelding",
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*"
            };
            
            if (openFileDialog.ShowDialog() == true)
            {
                ImagePathTextBox.Text = openFileDialog.FileName;
            }
        }
    }
}
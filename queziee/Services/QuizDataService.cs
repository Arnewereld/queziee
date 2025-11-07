using System.IO;
using System.Text.Json;
using queziee.Models;

namespace queziee.Services
{
    public class QuizDataService
    {
        private const string DataFileName = "quizdata.json";
        private readonly string _dataFilePath;
        
        public QuizDataService()
        {
            // Get solution directory by going up from bin folder
            // bin\Debug\net8.0-windows -> up 3 levels -> project root
            var binDirectory = AppDomain.CurrentDomain.BaseDirectory;
            
            // Navigate up: bin\Debug\net8.0-windows -> bin\Debug -> bin -> queziee (project root)
            var projectRoot = Path.GetFullPath(Path.Combine(binDirectory, @"..\..\..\"));
            
            _dataFilePath = Path.Combine(projectRoot, DataFileName);
            
            // DEBUG: Show EXACT file path
            System.Diagnostics.Debug.WriteLine($"?? Data file locatie: {_dataFilePath}");
            System.Diagnostics.Debug.WriteLine($"?? File bestaat: {File.Exists(_dataFilePath)}");
        }
        
        public async Task<List<Quiz>> GetQuizzesAsync()
        {
            if (!File.Exists(_dataFilePath))
            {
                System.Diagnostics.Debug.WriteLine($"?? File bestaat niet: {_dataFilePath}");
                return new List<Quiz>();
            }
            
            try
            {
                var json = await File.ReadAllTextAsync(_dataFilePath);
                var quizzes = JsonSerializer.Deserialize<List<Quiz>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                System.Diagnostics.Debug.WriteLine($"? {quizzes?.Count ?? 0} quizzen geladen");
                return quizzes ?? new List<Quiz>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Error loading: {ex.Message}");
                return new List<Quiz>();
            }
        }
        
        public async Task SaveQuizzesAsync(List<Quiz> quizzes)
        {
            var json = JsonSerializer.Serialize(quizzes, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            await File.WriteAllTextAsync(_dataFilePath, json);
            System.Diagnostics.Debug.WriteLine($"?? {quizzes.Count} quizzen opgeslagen naar: {_dataFilePath}");
        }
        
        public async Task<Quiz?> GetQuizByIdAsync(int id)
        {
            var quizzes = await GetQuizzesAsync();
            return quizzes.FirstOrDefault(q => q.Id == id);
        }
        
        public async Task AddQuizAsync(Quiz quiz)
        {
            var quizzes = await GetQuizzesAsync();
            quiz.Id = quizzes.Any() ? quizzes.Max(q => q.Id) + 1 : 1;
            quizzes.Add(quiz);
            await SaveQuizzesAsync(quizzes);
            System.Diagnostics.Debug.WriteLine($"? Quiz '{quiz.Name}' toegevoegd");
        }
        
        public async Task UpdateQuizAsync(Quiz quiz)
        {
            var quizzes = await GetQuizzesAsync();
            var index = quizzes.FindIndex(q => q.Id == quiz.Id);
            if (index >= 0)
            {
                quizzes[index] = quiz;
                await SaveQuizzesAsync(quizzes);
                System.Diagnostics.Debug.WriteLine($"?? Quiz '{quiz.Name}' geupdatet");
            }
        }
        
        public async Task DeleteQuizAsync(int id)
        {
            var quizzes = await GetQuizzesAsync();
            var quiz = quizzes.FirstOrDefault(q => q.Id == id);
            if (quiz != null)
            {
                quizzes.Remove(quiz);
                await SaveQuizzesAsync(quizzes);
                System.Diagnostics.Debug.WriteLine($"??? Quiz verwijderd");
            }
        }
    }
}
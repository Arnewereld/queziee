using System.ComponentModel.DataAnnotations;

namespace queziee.Models
{
    public class Question
    {
        public int Id { get; set; }
        
        [Required]
        public string Text { get; set; } = string.Empty;
        
        public string? ImagePath { get; set; }
        
        [Required]
        public string AnswerA { get; set; } = string.Empty;
        
        [Required]
        public string AnswerB { get; set; } = string.Empty;
        
        [Required]
        public string AnswerC { get; set; } = string.Empty;
        
        [Required]
        public string AnswerD { get; set; } = string.Empty;
        
        [Required]
        public char CorrectAnswer { get; set; } // A, B, C, or D
        
        public int QuizId { get; set; }
        
        public Quiz? Quiz { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace queziee.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public int TimePerQuestion { get; set; } = 30; // seconds
        
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
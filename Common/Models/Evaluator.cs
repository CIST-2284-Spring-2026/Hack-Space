using System.ComponentModel.DataAnnotations;
 
namespace Common.Models
{
    public class Evaluator
    {
        public Guid Id { get; set; } = Guid.Empty;
 
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; } = string.Empty;
 
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
 
        public string Background { get; set; } = string.Empty;
    }
}
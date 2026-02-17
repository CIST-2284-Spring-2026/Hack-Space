using System.ComponentModel.DataAnnotations;
 
namespace Common.Models
{
    public class Badge
    {
        public Guid Id { get; set; } = Guid.Empty;
 
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; } = string.Empty;
 
        [Required]
        [MinLength(20)]
        public string Description { get; set; } = string.Empty;
 
        [Required]
        [MinLength(20)]
        public string TurnInInstructions { get; set; } = string.Empty;
        public bool IsVisible { get; set; } = false;
    }
}
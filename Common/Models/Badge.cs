namespace Common.Models
{
    public class Badge
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TurnInInstructions { get; set; } = string.Empty;
        public bool IsVisible { get; set; } = false;
    }
}
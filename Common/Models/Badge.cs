public class Badge
{
    public Guid Id { get; set; } = Guid.Empty;

public string? Title { get; set; } 

public string? Description { get; set; } 

public string? TurnInInstructions { get; set; }

public bool? IsVisible { get; set; } = false;

}
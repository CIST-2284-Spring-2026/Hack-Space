namespace Common.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        public string Password { get; set; } = string.Empty;
        public List<RoleSelection> RoleSelections { get; set; } = new();
    }
}
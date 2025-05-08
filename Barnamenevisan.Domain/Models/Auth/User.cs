namespace Barnamenevisan.Domain.Models.Auth;

public class User:BaseEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public bool IsAdmin { get; set; }
    public string? ConfirmationCode { get; set; } = null;
}
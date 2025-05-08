namespace Barnamenevisan.Domain.ViewModels.Admin.User;

public class UserAdminViewModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsAdmin { get; set; }
    public int Id { get; set; }
    public DateTime RegisterDate { get; set; }
    public bool IsDeleted { get; set; }
}
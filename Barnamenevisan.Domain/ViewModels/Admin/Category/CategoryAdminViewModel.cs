namespace Barnamenevisan.Domain.ViewModels.Admin.Category;

public class CategoryAdminViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime RegisterDate { get; set; }
    public bool IsDeleted { get; set; }
}
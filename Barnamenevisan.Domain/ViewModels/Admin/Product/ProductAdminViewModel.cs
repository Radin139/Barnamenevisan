namespace Barnamenevisan.Domain.ViewModels.Admin.Product;

public class ProductAdminViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Price { get; set; }
    public string Category { get; set; }
    public DateTime RegisterDate { get; set; }
    public bool IsDeleted { get; set; }
}
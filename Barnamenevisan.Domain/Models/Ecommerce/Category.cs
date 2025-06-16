namespace Barnamenevisan.Domain.Models.Ecommerce;

public class Category:BaseEntity
{
    public string Title { get; set; }
    public ICollection<Product>? Products { get; set; }
}
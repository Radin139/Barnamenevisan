using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;

namespace Barnamenevisan.Domain.Models.Ecommerce;

public class Product:BaseEntity
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }
    public int Price { get; set; }
    public string? Tags { get; set; }
    public int CategoryId { get; set; }
    
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }

    public ICollection<ProductImage> Images { get; set; }
}
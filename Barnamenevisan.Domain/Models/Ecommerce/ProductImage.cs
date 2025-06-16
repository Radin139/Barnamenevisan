using System.ComponentModel.DataAnnotations.Schema;

namespace Barnamenevisan.Domain.Models.Ecommerce;

public class ProductImage:BaseEntity
{
    public string ImageName { get; set; }
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
}
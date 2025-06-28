using Barnamenevisan.Domain.Models.Ecommerce;

namespace Barnamenevisan.Domain.Interfaces;

public interface IProductRepository:IRepository<Product>
{
    Task<List<Product>> GetAllProductsWithCategoriesAsync();
}
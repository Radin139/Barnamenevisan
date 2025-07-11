using Barnamenevisan.Domain.Models.Ecommerce;

namespace Barnamenevisan.Domain.Interfaces;

public interface IProductRepository:IRepository<Product>
{
    Task<List<Product>> GetAllProductsWithIncludeAsync();
    Task AddProductWithImagesAsync(Product product, List<string> imageNames);
    Task<List<ProductImage>> GetProductImagesAsync(int id);
    Task<ProductImage?> GetProductImageByIdAsync(int id);
    void DeleteProductImage(ProductImage image);
    Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
}
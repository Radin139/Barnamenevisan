using Barnamenevisan.Data.Context;
using Barnamenevisan.Domain.Interfaces;
using Barnamenevisan.Domain.Models.Ecommerce;
using Microsoft.EntityFrameworkCore;

namespace Barnamenevisan.Data.Repositories;

public class ProductRepository(BarnamenevisanDbContext context) : Repository<Product>(context), IProductRepository
{
    public async Task<List<Product>> GetAllProductsWithIncludeAsync()
    {
        return await context.Products.Include(product => product.Category).Include(product => product.Images).ToListAsync();
    }

    public async Task AddProductWithImagesAsync(Product product, List<string> imageNames)
    {
        product.Images = new List<ProductImage>();
        
        foreach (var imageName in imageNames)
        {
            product.Images.Add(new ProductImage()
            {
                ImageName = imageName,
                IsDeleted = false,
                RegisterDate = DateTime.Now
            });
        }
        
        await context.Products.AddAsync(product);
    }
}
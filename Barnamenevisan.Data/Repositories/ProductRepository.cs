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

    public async Task<List<ProductImage>> GetProductImagesAsync(int id)
    {
        var list = await context.ProductImages.Where(productImage => productImage.ProductId == id).ToListAsync();
        
        return list;
    }

    public async Task<ProductImage?> GetProductImageByIdAsync(int id)
    {
        return await context.ProductImages.FirstOrDefaultAsync(productImage => productImage.Id == id);
    }

    public void DeleteProductImage(ProductImage image)
    {
        context.ProductImages.Remove(image);
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        return await context.Products.Include(product => product.Images).Where(product => product.CategoryId == categoryId).ToListAsync();
    }
}
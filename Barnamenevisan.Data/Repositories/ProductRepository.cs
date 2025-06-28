using Barnamenevisan.Data.Context;
using Barnamenevisan.Domain.Interfaces;
using Barnamenevisan.Domain.Models.Ecommerce;
using Microsoft.EntityFrameworkCore;

namespace Barnamenevisan.Data.Repositories;

public class ProductRepository(BarnamenevisanDbContext context) : Repository<Product>(context), IProductRepository
{
    public async Task<List<Product>> GetAllProductsWithCategoriesAsync()
    {
        return await context.Products.Include(product => product.Category).ToListAsync();
    }
}
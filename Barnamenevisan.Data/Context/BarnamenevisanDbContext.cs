using Barnamenevisan.Domain.Models.Auth;
using Barnamenevisan.Domain.Models.Ecommerce;
using Microsoft.EntityFrameworkCore;

namespace Barnamenevisan.Data.Context;

public class BarnamenevisanDbContext:DbContext
{
    #region DbSets

    
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<SliderImage> SliderImages { get; set; }

    #endregion

    #region Constructor

    public BarnamenevisanDbContext(DbContextOptions<BarnamenevisanDbContext> options)
        : base(options)
    {
    }

    #endregion
}
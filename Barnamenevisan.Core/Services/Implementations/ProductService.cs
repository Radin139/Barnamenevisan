using Barnamenevisan.Core.Services.Interfaces;
using Barnamenevisan.Domain.Interfaces;
using Barnamenevisan.Domain.Models.Ecommerce;
using Barnamenevisan.Domain.ViewModels.Admin.Product;
using Microsoft.IdentityModel.Tokens;

namespace Barnamenevisan.Core.Services.Implementations;

public class ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository):IProductService
{
    public async Task<List<ProductAdminViewModel>> GetProductsForAdminAsync()
    {
        var list = await productRepository.GetAllProductsWithCategoriesAsync();
        return list.Select(product => new ProductAdminViewModel
        {
            Id = product.Id,
            Category = product.Category.Title,
            Title = product.Title,
            IsDeleted = product.IsDeleted,
            Price = product.Price,
            RegisterDate = product.RegisterDate
        }).ToList();
    }

    public async Task<ProductCreateResult> CreateProductAsync(ProductCreateViewModel viewModel)
    {
        Category? category = await categoryRepository.GetByIdAsync(viewModel.CategoryId);

        if (category == null)
        {
            return ProductCreateResult.CategoryNotFound;
        }
        
        Product product = new Product()
        {
            CategoryId = viewModel.CategoryId,
            Title = viewModel.Title,
            Price = viewModel.Price,
            LongDescription = viewModel.LongDescription,
            ShortDescription = viewModel.ShortDescription,
            Tags = string.IsNullOrWhiteSpace(viewModel.Tags) ? null : viewModel.Tags,
            IsDeleted = false,
            RegisterDate = DateTime.Now,
            Images = new List<ProductImage>(),
        };

        await productRepository.InsertAsync(product);
        await productRepository.SaveAsync();

        return ProductCreateResult.Success;
    }
}
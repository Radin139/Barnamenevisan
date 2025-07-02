using Barnamenevisan.Domain.ViewModels.Admin.Product;
using Barnamenevisan.Domain.ViewModels.Ecommerce;

namespace Barnamenevisan.Core.Services.Interfaces;

public interface IProductService
{
    Task<List<ProductAdminViewModel>> GetProductsForAdminAsync();
    Task<ProductCreateResult> CreateProductAsync(ProductCreateViewModel viewModel, List<string> imageNames);
    Task<List<ProductDisplayViewModel>> GetProductsForDisplayAsync();
}
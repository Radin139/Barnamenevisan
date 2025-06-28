using Barnamenevisan.Domain.ViewModels.Admin.Product;

namespace Barnamenevisan.Core.Services.Interfaces;

public interface IProductService
{
    Task<List<ProductAdminViewModel>> GetProductsForAdminAsync();
    Task<ProductCreateResult> CreateProductAsync(ProductCreateViewModel viewModel);
    
}
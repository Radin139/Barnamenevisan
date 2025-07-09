using Barnamenevisan.Domain.ViewModels.Admin.Product;
using Barnamenevisan.Domain.ViewModels.Ecommerce;

namespace Barnamenevisan.Core.Services.Interfaces;

public interface IProductService
{
    Task<List<ProductAdminViewModel>> GetProductsForAdminAsync();
    Task<ProductCreateResult> CreateProductAsync(ProductCreateViewModel viewModel, List<string> imageNames);
    Task<List<ProductDisplayViewModel>> GetProductsForDisplayAsync();
    Task<ProductEditViewModel?> GetProductForEditAsync(int id);
    Task<EditProductResult> EditProductAsync(ProductEditViewModel viewModel);
    Task DeleteImageAsync(int id);
    Task<bool> DeleteProductAsync(int id);
    Task<bool> RestoreProductAsync(int id);
    Task<ProductDeletePermanentlyViewModel?> GetProductForPermanentlyDeleteAsync(int id);  
    Task<bool> DeleteProductPermanentlyAsync(int id);
}
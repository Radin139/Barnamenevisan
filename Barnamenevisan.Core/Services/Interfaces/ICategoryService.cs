using Barnamenevisan.Domain.ViewModels.Admin.Category;
using Barnamenevisan.Domain.ViewModels.Blog;

namespace Barnamenevisan.Core.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDisplayViewModel>> GetCategoriesForDisplayAsync();
    Task<List<CategoryAdminViewModel>> GetCategoriesForAdminAsync();
    Task CreateCategoryAsync(CategoryCreateViewModel model);
    Task<CategoryEditViewModel?> GetCategoryForEditAsync(int id);
    Task<bool> EditCategoryAsync(CategoryEditViewModel viewModel);
    Task<bool> DeleteCategoryAsync(int id);
    Task<bool> RestoreCategoryAsync(int id);
    Task<CategoryDeletePermanentlyViewModel?> GetCategoryForPermanentlyDeleteAsync(int id);  
    Task<bool> DeleteCategoryPermanentlyAsync(int id);
}
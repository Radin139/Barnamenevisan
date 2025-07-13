using Barnamenevisan.Core.Services.Interfaces;
using Barnamenevisan.Domain.Interfaces;
using Barnamenevisan.Domain.Models.Ecommerce;
using Barnamenevisan.Domain.ViewModels.Admin.Category;
using Barnamenevisan.Domain.ViewModels.Blog;
using Barnamenevisan.Domain.ViewModels.Ecommerce;

namespace Barnamenevisan.Core.Services.Implementations;

public class CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository):ICategoryService
{
    public async Task<List<CategoryDisplayViewModel>> GetCategoriesForDisplayAsync()
    {
        var list = await categoryRepository.GetAllAsync();

        return list.Where(category => category.IsDeleted == false).Select(category => new CategoryDisplayViewModel()
        {
            Id = category.Id,
            Title = category.Title,
        }).OrderBy(model => model.Title).ToList();
    }

    public async Task<List<CategoryAdminViewModel>> GetCategoriesForAdminAsync()
    {
        var list = await categoryRepository.GetAllAsync();
        
        var result =  list.Select(category => new CategoryAdminViewModel()
        {
            Id = category.Id,
            Title = category.Title,
            RegisterDate = category.RegisterDate,
            IsDeleted = category.IsDeleted
        }).ToList();
        
        return result
            .OrderBy(user => user.IsDeleted)
            .ThenByDescending(user => user.RegisterDate)
            .ToList();
    }

    public async Task CreateCategoryAsync(CategoryCreateViewModel model)
    {
        Category category = new Category
        {
            IsDeleted = false,
            Title = model.Title,
            RegisterDate = DateTime.Now
        };

        await categoryRepository.InsertAsync(category);
        await categoryRepository.SaveAsync();
    }

    public async Task<CategoryEditViewModel?> GetCategoryForEditAsync(int id)
    {
        Category? category = await categoryRepository.GetByIdAsync(c => c.Id == id && !c.IsDeleted);

        if (category == null)
        {
            return null;
        }

        return new CategoryEditViewModel()
        {
            Id = category.Id,
            Title = category.Title,
        };
    }

    public async Task<bool> EditCategoryAsync(CategoryEditViewModel viewModel)
    {
        Category? category = await categoryRepository.GetByIdAsync(c => c.Id == viewModel.Id && !c.IsDeleted);

        if (category == null)
        {
            return false;
        }
        
        category.Title = viewModel.Title;
        
        categoryRepository.Update(category);
        await categoryRepository.SaveAsync();

        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        Category? category = await categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return false;
        }
        
        category.IsDeleted = true;
        categoryRepository.Update(category);
        await categoryRepository.SaveAsync();
        
        return true;
    }

    public async Task<bool> RestoreCategoryAsync(int id)
    {
        Category? category = await categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return false;
        }
        
        category.IsDeleted = false;
        categoryRepository.Update(category);
        await categoryRepository.SaveAsync();
        
        return true;
    }

    public async Task<CategoryDeletePermanentlyViewModel?> GetCategoryForPermanentlyDeleteAsync(int id)
    {
        Category? category = await categoryRepository.GetByIdAsync(c => c.Id == id && c.IsDeleted);
        if (category == null)
        {
            return null;
        }

        return new CategoryDeletePermanentlyViewModel()
        {
            Id = category.Id,
            Title = category.Title,
        };
    }

    public async Task<bool> DeleteCategoryPermanentlyAsync(int id)
    {
        Category? category = await categoryRepository.GetByIdAsync(c => c.Id == id && c.IsDeleted);
        if (category == null)
        {
            return false;
        }
        
        var products = await productRepository.GetProductsByCategoryAsync(category.Id);

        foreach (var product in products)
        {
            await productRepository.DeleteProductWithImagesAsync(product);
        }
        
        categoryRepository.Delete(category);
        await categoryRepository.SaveAsync();
        
        return true;
    }

    public async Task<CategoryProductsViewModel?> GetCategoryProductsAsync(int id)
    {
        Category? category = await categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            return null;
        }

        var list = await productRepository.GetProductsByCategoryAsync(category.Id);
        var products = list.Where(product => !product.IsDeleted).OrderByDescending(product => product.RegisterDate)
            .Select(product => new ProductDisplayViewModel()
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                Image = product.Images.First().ImageName
            }).ToList();

        return new CategoryProductsViewModel()
        {
            Products = products,
            Title = category.Title,
        };
    }
}
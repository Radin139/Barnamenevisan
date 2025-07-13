using Barnamenevisan.Core.Services.Interfaces;
using Barnamenevisan.Domain.Interfaces;
using Barnamenevisan.Domain.Models.Ecommerce;
using Barnamenevisan.Domain.ViewModels.Admin.Product;
using Barnamenevisan.Domain.ViewModels.Ecommerce;
using Microsoft.IdentityModel.Tokens;

namespace Barnamenevisan.Core.Services.Implementations;

public class ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository):IProductService
{
    public async Task<List<ProductAdminViewModel>> GetProductsForAdminAsync()
    {
        var list = await productRepository.GetAllProductsWithIncludeAsync();
        return list.OrderBy(product => product.IsDeleted).ThenByDescending(product => product.RegisterDate).Select(product => new ProductAdminViewModel
        {
            Id = product.Id,
            Category = product.Category.Title,
            Title = product.Title,
            IsDeleted = product.IsDeleted,
            Price = product.Price,
            RegisterDate = product.RegisterDate
        }).ToList();
    }

    public async Task<ProductCreateResult> CreateProductAsync(ProductCreateViewModel viewModel, List<string> imageNames)
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

        await productRepository.AddProductWithImagesAsync(product, imageNames);
        await productRepository.SaveAsync();

        return ProductCreateResult.Success;
    }

    public async Task<List<ProductDisplayViewModel>> GetProductsForDisplayAsync()
    {
        var list = await productRepository.GetAllProductsWithIncludeAsync();
        return list.Where(product => !product.IsDeleted).OrderByDescending(product => product.RegisterDate)
            .Take(12).Select(product => new ProductDisplayViewModel()
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                Image = product.Images.First().ImageName
            }).ToList();
    }

    public async Task<ProductEditViewModel?> GetProductForEditAsync(int id)
    {
        Product? product = await productRepository.GetByIdAsync(p => p.Id == id && !p.IsDeleted);

        if (product == null)
        {
            return null;
        }

        return new ProductEditViewModel()
        {
            Id = product.Id,
            Title = product.Title,
            Price = product.Price,
            LongDescription = product.LongDescription,
            ShortDescription = product.ShortDescription,
            Tags = product.Tags,
            CategoryId = product.CategoryId,
            Images = await productRepository.GetProductImagesAsync(product.Id)
        };
    }

    public async Task DeleteImageAsync(int id)
    {
        ProductImage? image = await productRepository.GetProductImageByIdAsync(id);

        if (image != null)
        {
            productRepository.DeleteProductImage(image);
            
            string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", image.ImageName);

            if (File.Exists(imgPath))
            {
                File.Delete(imgPath);
            }
            
            await productRepository.SaveAsync();
        }
        
        
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        Product? product = await productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }
        
        product.IsDeleted = true;
        productRepository.Update(product);
        await categoryRepository.SaveAsync();
        
        return true;
    }

    public async Task<bool> RestoreProductAsync(int id)
    {
        Product? product = await productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }
        
        product.IsDeleted = false;
        productRepository.Update(product);
        await categoryRepository.SaveAsync();
        
        return true;
    }

    public async Task<ProductDeletePermanentlyViewModel?> GetProductForPermanentlyDeleteAsync(int id)
    {
        Product? product = await productRepository.GetByIdAsync(p => p.Id == id && p.IsDeleted);
        if (product == null)
        {
            return null;
        }

        return new ProductDeletePermanentlyViewModel()
        {
            Id = product.Id,
            Title = product.Title,
        };
    }

    public async Task<bool> DeleteProductPermanentlyAsync(int id)
    {
        Product? product = await productRepository.GetByIdAsync(p => p.Id == id && p.IsDeleted);
        if (product == null)
        {
            return false;
        }
        
        await productRepository.DeleteProductWithImagesAsync(product);
        await categoryRepository.SaveAsync();
        
        return true;
    }

    public async Task<EditProductResult> EditProductAsync(ProductEditViewModel viewModel)
    {
        Product? product = await productRepository.GetByIdAsync(viewModel.Id);

        if (product == null)
        {
            return EditProductResult.ProductNotFound;
        }

        Category? category = await categoryRepository.GetByIdAsync(viewModel.CategoryId);
        if (category == null)
        {
            return EditProductResult.CategoryNotFound;
        }
        
        product.Title = viewModel.Title;
        product.Price = viewModel.Price;
        product.LongDescription = viewModel.LongDescription;
        product.ShortDescription = viewModel.ShortDescription;
        product.Tags = viewModel.Tags ?? "";
        product.Images = viewModel.Images;
        product.CategoryId = viewModel.CategoryId;
        
        productRepository.Update(product);
        await productRepository.SaveAsync();
        
        return EditProductResult.Success;
    }
}
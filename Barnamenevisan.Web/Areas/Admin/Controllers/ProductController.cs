using Barnamenevisan.Core.Services.Interfaces;
using Barnamenevisan.Domain.Models.Ecommerce;
using Barnamenevisan.Domain.ViewModels.Admin.Category;
using Barnamenevisan.Domain.ViewModels.Admin.Product;
using Barnamenevisan.Domain.ViewModels.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Barnamenevisan.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProductController(IProductService productService, ICategoryService categoryService) : Controller
{
    private async Task getCategoriesForCreate()
    {
        var categories = await categoryService.GetCategoriesForDisplayAsync();
        List<SelectListItem> selectList = new List<SelectListItem>();
        
        foreach (CategoryDisplayViewModel category in categories)
        {
            selectList.Add(new(category.Title, category.Id.ToString()));
        }
        
        ViewBag.Categories = selectList;
    }
    
    private async Task getCategoriesForEdit(int categoryId)
    {
        var categories = await categoryService.GetCategoriesForDisplayAsync();
        List<SelectListItem> selectList = new List<SelectListItem>();
        
        foreach (CategoryDisplayViewModel category in categories)
        {
            selectList.Add(new(category.Title, category.Id.ToString(), category.Id == categoryId));
        }
        
        ViewBag.Categories = selectList;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var list = await productService.GetProductsForAdminAsync();
        return View(list);
    }

    public async Task<IActionResult> Create()
    {
        await getCategoriesForCreate();
        
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateViewModel viewModel, IFormFile[]? imgUp)
    {
        await getCategoriesForCreate();
        
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        if (imgUp == null || !imgUp.Any())
        {
            ModelState.AddModelError("Image", "حداقل یک تصویر الزامی است");
            return View(viewModel);
        }
        
        List<string> imageNames = new List<string>();
        
        foreach (IFormFile img in imgUp)
        {
            string imageName = Guid.NewGuid() + Path.GetExtension(img.FileName);   
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", imageName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            string savePath = Path.Combine(folderPath, imageName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await img.CopyToAsync(stream);
            }
            imageNames.Add(imageName);
        }

        var result = await productService.CreateProductAsync(viewModel, imageNames);

        if (result == ProductCreateResult.CategoryNotFound)
        {
            ModelState.AddModelError("CategoryId", "دسته بندی پیدا نشد");
            return View(viewModel);
        }
        
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        ProductEditViewModel? viewModel = await productService.GetProductForEditAsync(id);

        if (viewModel == null)
        {
            return NotFound();
        }
        
        await getCategoriesForEdit(viewModel.CategoryId);
        
        return View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductEditViewModel viewModel, IFormFile[]? imgUp)
    {
        await getCategoriesForEdit(viewModel.CategoryId);
        
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        
        ProductEditViewModel? mainModel = await productService.GetProductForEditAsync(viewModel.Id);

        if (mainModel == null)
        {
            return View(viewModel);
        }

        if ((imgUp == null || !imgUp.Any()) && (!mainModel.Images.Any() || mainModel.Images == null))
        {
            ModelState.AddModelError("Image", "حداقل یک تصویر الزامی است");
            return View(viewModel);
        }

        List<ProductImage> productImages;

        if (imgUp == null || !imgUp.Any())
        {
            productImages = mainModel.Images.ToList();
        }
        else
        {
            productImages = mainModel.Images.ToList();
            
            foreach (IFormFile img in imgUp)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            
                string imageName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                string savePath = Path.Combine(folderPath, imageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }

                productImages.Add(new ProductImage()
                {
                    ImageName = imageName,
                    IsDeleted = false,
                    ProductId = mainModel.Id,
                    RegisterDate = DateTime.Now
                });
            }
        }
        
        viewModel.Images = productImages;
        
        EditProductResult result = await productService.EditProductAsync(viewModel);

        switch (result)
        {
            case EditProductResult.Success:
                return RedirectToAction(nameof(Index));
            case EditProductResult.CategoryNotFound:
                ModelState.AddModelError("CategoryId", "دسته بندی پیدا نشد");
                return View(viewModel);
            case EditProductResult.ProductNotFound:
                return View(viewModel);
            default:
                return View(viewModel);
        }
    }

    public async Task DeleteImage(int id)
    {
        await productService.DeleteImageAsync(id);
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        bool result = await productService.DeleteProductAsync(id);
        if (result)
        {
            return RedirectToAction("Index");
        }
        
        return NotFound();
    }
    
    public async Task<IActionResult> Restore(int id)
    {
        bool result = await productService.RestoreProductAsync(id);
        if (result)
        {
            return RedirectToAction("Index");
        }
        
        return NotFound();
    }
    
    public async Task<IActionResult> DeletePermanently(int id)
    {
        ProductDeletePermanentlyViewModel? viewModel = await productService.GetProductForPermanentlyDeleteAsync(id);

        if (viewModel == null)
        {
            return NotFound();
        }
        
        return View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePermanently(ProductDeletePermanentlyViewModel viewModel)
    {
        bool result = await productService.DeleteProductPermanentlyAsync(viewModel.Id);

        if (!result)
        {
            return View(viewModel);
        }
        
        return RedirectToAction("Index");
    }
}
using Barnamenevisan.Core.Services.Interfaces;
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
    // GET
    public async Task<IActionResult> Index()
    {
        var list = await productService.GetProductsForAdminAsync();
        return View(list);
    }

    public async Task<IActionResult> Create()
    {
        var categories = await categoryService.GetCategoriesForDisplayAsync();
        List<SelectListItem> selectList = new List<SelectListItem>();
        
        foreach (CategoryDisplayViewModel category in categories)
        {
            selectList.Add(new(category.Title, category.Id.ToString()));
        }
        
        ViewBag.Categories = selectList;
        
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateViewModel viewModel, IFormFile[]? imgUp)
    {
        var categories = await categoryService.GetCategoriesForDisplayAsync();
        List<SelectListItem> selectList = new List<SelectListItem>();
        
        foreach (CategoryDisplayViewModel category in categories)
        {
            selectList.Add(new(category.Title, category.Id.ToString()));
        }
        
        ViewBag.Categories = selectList;
        
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

    public async Task<IActionResult> ChooseThumbnail(int id)
    {
        return View();
    }
}
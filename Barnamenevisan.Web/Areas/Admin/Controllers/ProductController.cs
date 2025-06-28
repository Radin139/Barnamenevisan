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
    public async Task<IActionResult> Create(ProductCreateViewModel viewModel)
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

        var result = await productService.CreateProductAsync(viewModel);

        if (result == ProductCreateResult.CategoryNotFound)
        {
            ModelState.AddModelError("CategoryId", "دسته بندی پیدا نشد");
            return View(viewModel);
        }
        
        return RedirectToAction(nameof(Index));
    }
}
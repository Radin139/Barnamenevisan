using System.Diagnostics;
using Barnamenevisan.Core.Services.Interfaces;
using Barnamenevisan.Domain.ViewModels.Ecommerce;
using Microsoft.AspNetCore.Mvc;

namespace Barnamenevisan.Web.Controllers;

public class HomeController(IProductService productService, ICategoryService categoryService) : Controller
{

    public async Task<IActionResult> Index()
    {
        var list = await productService.GetProductsForDisplayAsync();
        return View(list);
    }

    [Route("category/{id}")]
    public async Task<IActionResult> Category(int id)
    {
        CategoryProductsViewModel? viewModel = await categoryService.GetCategoryProductsAsync(id);

        if (viewModel == null)
        {
            return NotFound();
        }
        
        return View(viewModel);
    }
}
using System.Diagnostics;
using Barnamenevisan.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Barnamenevisan.Web.Controllers;

public class HomeController(IProductService productService) : Controller
{

    public async Task<IActionResult> Index()
    {
        var list = await productService.GetProductsForDisplayAsync();
        return View(list);
    }
}
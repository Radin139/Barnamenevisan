using Barnamenevisan.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Barnamenevisan.Web.ViewComponents;

[ViewComponent]
public class CategoryDisplayViewComponent(ICategoryService categoryService):ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var list = await categoryService.GetCategoriesForDisplayAsync();
        return View(list);
    }
}
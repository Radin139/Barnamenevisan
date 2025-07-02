using Barnamenevisan.Domain.ViewModels.Ecommerce;
using Microsoft.AspNetCore.Mvc;

namespace Barnamenevisan.Web.ViewComponents;

[ViewComponent]
public class ProductCardViewComponent:ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(ProductDisplayViewModel viewModel)
    {
        return View(viewModel);
    }
}
using Barnamenevisan.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Barnamenevisan.Web.ViewComponents;

[ViewComponent]
public class SliderViewComponent(ISliderImageService sliderImageService):ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        List<string> imageNames = await sliderImageService.GetSliderImagesForDisplayAsync();
        return View(imageNames);
    }
}
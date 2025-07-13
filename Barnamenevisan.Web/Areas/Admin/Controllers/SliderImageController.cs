using Barnamenevisan.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barnamenevisan.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class SliderImageController(ISliderImageService sliderImageService) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var list = await sliderImageService.GetSliderImagesForAdminAsync();
        return View(list);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IFormFile[]? imgUp)
    {
        if (imgUp != null && imgUp.Any())
        {
            List<string> imageNames = new List<string>();

            foreach (var img in imgUp)
            {
                string imageName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");

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

            await sliderImageService.AddSliderImagesAsync(imageNames);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        bool result = await sliderImageService.DeleteSliderImageAsync(id);
        if (result)
        {
            return RedirectToAction(nameof(Index));
        }

        return NotFound();
    }

    public async Task<IActionResult> Restore(int id)
    {
        bool result = await sliderImageService.RestoreSliderImageAsync(id);
        if (result)
        {
            return RedirectToAction(nameof(Index));
        }

        return NotFound();
    }

    public async Task<IActionResult> DeletePermanently(int id)
    {
        bool result = await sliderImageService.DeleteSliderImagePermanentlyAsync(id);
        if (result)
        {
            return RedirectToAction(nameof(Index));
        }

        return NotFound();
    }
}
using Barnamenevisan.Core.Services.Interfaces;
using Barnamenevisan.Domain.Interfaces;
using Barnamenevisan.Domain.Models.Ecommerce;
using Barnamenevisan.Domain.ViewModels.Ecommerce;

namespace Barnamenevisan.Core.Services.Implementations;

public class SliderImageService(ISliderImageRepository sliderImageRepository):ISliderImageService
{
    public async Task<List<SliderImageAdminViewModel>> GetSliderImagesForAdminAsync()
    {
        var list = await sliderImageRepository.GetAllAsync();

        return list.OrderBy(image => image.IsDeleted).ThenByDescending(image => image.RegisterDate).Select(image =>
            new SliderImageAdminViewModel
            {
                ImageName = image.ImageName,
                IsDeleted = image.IsDeleted,
                RegisterDate = image.RegisterDate,
                Id = image.Id,
            }).ToList();
    }

    public async Task AddSliderImagesAsync(List<string> imageNames)
    {
        foreach (string imageName in imageNames)
        {
            await sliderImageRepository.InsertAsync(new SliderImage
            {
                RegisterDate = DateTime.Now,
                ImageName = imageName,
                IsDeleted = false,
            });
            
            await sliderImageRepository.SaveAsync();
        }
    }

    public async Task<List<string>> GetSliderImagesForDisplayAsync()
    {
        var list = await sliderImageRepository.GetAllAsync();
        return list.Where(image => !image.IsDeleted).Select(image => image.ImageName).ToList();
    }

    public async Task<bool> DeleteSliderImageAsync(int id)
    {
        SliderImage? sliderImage = await sliderImageRepository.GetByIdAsync(image => image.Id == id && !image.IsDeleted);

        if (sliderImage == null)
        {
            return false;
        }

        sliderImage.IsDeleted = true;
        sliderImageRepository.Update(sliderImage);
        await sliderImageRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteSliderImagePermanentlyAsync(int id)
    {
        SliderImage? sliderImage = await sliderImageRepository.GetByIdAsync(image => image.Id == id && image.IsDeleted);

        if (sliderImage == null)
        {
            return false;
        }

        string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", sliderImage.ImageName);

        if (File.Exists(imgPath))
        {
            File.Delete(imgPath);
        }

        sliderImageRepository.Delete(sliderImage);
        await sliderImageRepository.SaveAsync();
        return true;
    }

    public async Task<bool> RestoreSliderImageAsync(int id)
    {
        SliderImage? sliderImage = await sliderImageRepository.GetByIdAsync(image => image.Id == id && image.IsDeleted);

        if (sliderImage == null)
        {
            return false;
        }

        sliderImage.IsDeleted = false;
        sliderImageRepository.Update(sliderImage);
        await sliderImageRepository.SaveAsync();
        return true;
    }
}
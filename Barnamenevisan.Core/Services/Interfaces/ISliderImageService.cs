using Barnamenevisan.Domain.ViewModels.Ecommerce;

namespace Barnamenevisan.Core.Services.Interfaces;

public interface ISliderImageService
{
    Task<List<SliderImageAdminViewModel>> GetSliderImagesForAdminAsync();
    Task AddSliderImagesAsync(List<string> imageNames);
    Task<List<string>> GetSliderImagesForDisplayAsync();
    Task<bool> DeleteSliderImageAsync(int id);
    Task<bool> DeleteSliderImagePermanentlyAsync(int id);
    Task<bool> RestoreSliderImageAsync(int id);
}
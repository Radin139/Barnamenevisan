namespace Barnamenevisan.Domain.ViewModels.Admin.Product;

public class ChooseThumbnailViewModel
{
    public string CurrentThumbnail { get; set; }
    public List<ChooseThumbnailOptionsViewModel> Options { get; set; }
}
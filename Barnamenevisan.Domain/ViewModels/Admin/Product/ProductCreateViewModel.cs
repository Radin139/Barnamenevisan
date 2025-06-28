using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Barnamenevisan.Domain.ViewModels.Admin.Product;

public class ProductCreateViewModel
{
    [DisplayName("عنوان")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(350, ErrorMessage = "تعداد کاراکتر وارد شده بیش از حد مجاز میباشد")]
    public string Title { get; set; }
    
    [DisplayName("توضیحات کوتاه")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(800, ErrorMessage = "تعداد کاراکتر وارد شده بیش از حد مجاز میباشد")]
    public string ShortDescription { get; set; }
    
    [DisplayName("توضیحات کامل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string LongDescription { get; set; }
    
    [DisplayName("قیمت")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public int Price { get; set; }
    
    [DisplayName("برچسب ها")]
    public string? Tags { get; set; }
    
    [DisplayName("دسته بندی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public int CategoryId { get; set; }
}

public enum ProductCreateResult
{
    Success,
    CategoryNotFound,
}
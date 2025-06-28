using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Barnamenevisan.Domain.ViewModels.Admin.Category;

public class CategoryCreateViewModel
{
    [DisplayName("عنوان")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(300, ErrorMessage = "تعداد کاراکتر وارد شده بیش از حد مجاز میباشد")]
    public string Title { get; set; }
}
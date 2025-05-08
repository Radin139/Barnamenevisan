using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Barnamenevisan.Domain.ViewModels.Admin.User;

public class UserCreateViewModel
{
    [DisplayName("نام کاربری")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(150, ErrorMessage = "تعداد کاراکتر وارد شده بیش از حد مجاز میباشد")]
    public string Username { get; set; }
    
    [DisplayName("ایمیل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [EmailAddress(ErrorMessage = "نوع متن وارد شده اشتباه میباشد")]
    public string Email { get; set; }
    
    [DisplayName("رمز عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(150, ErrorMessage = "تعداد کاراکتر وارد شده بیش از حد مجاز میباشد")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [DisplayName("ادمین")]
    public bool IsAdmin { get; set; }
}

public enum CreateUserResult
{
    Success,
    UsernameExists,
}
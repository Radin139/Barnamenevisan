using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Barnamenevisan.Domain.ViewModels.Auth;

public class ResetPasswordViewModel
{
    [DisplayName("کد تایید")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(5, ErrorMessage = "تعداد کاراکتر وارد شده بیش از حد مجاز میباشد")]
    [MinLength(5, ErrorMessage = "تعداد کاراکتر وارد شده کمتر از حد مجاز میباشد")]
    [DataType(DataType.Password)]
    public string ConfirmCode { get; set; }
    
    [DisplayName("رمز عبور جدید")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(150, ErrorMessage = "تعداد کاراکتر وارد شده بیش از حد مجاز میباشد")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [DisplayName("تکرار رمز عبور جدید")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(150, ErrorMessage = "تعداد کاراکتر وارد شده بیش از حد مجاز میباشد")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "تکرار کلمه عبور صحیح نمیباشد")]
    public string ConfirmPassword { get; set; }
}

public enum ResetPasswordResult
{
    Success,
    WrongCode
}
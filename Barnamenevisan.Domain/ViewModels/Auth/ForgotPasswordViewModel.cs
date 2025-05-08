using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Barnamenevisan.Domain.ViewModels.Auth;

public class ForgotPasswordViewModel
{
    [DisplayName("ایمیل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [EmailAddress(ErrorMessage = "نوع متن وارد شده اشتباه میباشد")]
    public string Email { get; set; }
}

public enum ForgotPasswordResult
{
    Success,
    UserNotFound,
    EmailNotFound,
}
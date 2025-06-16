using System.Security.Claims;
using Barnamenevisan.Core.Services.Interfaces;
using Barnamenevisan.Domain.Models.Auth;
using Barnamenevisan.Domain.ViewModels.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Barnamenevisan.Web.Areas.Auth.Controllers;

[Area("Auth")]
public class AccountController(IUserService userService) : Controller
{
    #region Register
    
    [Route("register")]
    public IActionResult Register()
    {
        return View();
    }

    [Route("register")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        if (viewModel.ConfirmPolicies == false)
        {
            ModelState.AddModelError("ConfirmPolicies", "لطفا قوانین را بپذیرید");
            return View(viewModel);
        }

        RegisterResult result = await userService.RegisterUserAsync(viewModel);

        switch (result)
        {
            case RegisterResult.UsernameExists:
            {
                ModelState.AddModelError("Username", "نام کاربری تکراری است");
                return View(viewModel);
            }
            case RegisterResult.Success:
            {
                User user = await userService.GetUserByUsernameAsync(viewModel.Username);

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("IsAdmin", user.IsAdmin.ToString()),
                };
                
                ClaimsIdentity claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimPrincipal = new ClaimsPrincipal(claimIdentity);

                await HttpContext.SignInAsync(claimPrincipal, new AuthenticationProperties()
                {
                    IsPersistent = true,
                });
                
                TempData["Color"] = "success";
                TempData["Message"] = "ثبت نام با موفقیت انجام شد";
                return View("ShowMessage");
            }
        }
        
        return View(viewModel);
    }

    #endregion
    
    #region Login
    
    [Route("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [Route("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        
        LoginResult result = await userService.LoginUserAsync(viewModel);

        switch (result)
        {
            case LoginResult.UserNotFound:
            {
                ModelState.AddModelError("Username", "اطلاعات وارد شده اشتباه میباشد");
                return View(viewModel);
            }
            case LoginResult.Success:
            {
                User user = await userService.GetUserByUsernameAsync(viewModel.Username);

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("IsAdmin", user.IsAdmin.ToString()),
                };
                
                ClaimsIdentity claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimPrincipal = new ClaimsPrincipal(claimIdentity);

                await HttpContext.SignInAsync(claimPrincipal, new AuthenticationProperties()
                {
                    IsPersistent = true,
                });
                
                TempData["Color"] = "success";
                TempData["Message"] = "ورود با موفقیت انجام شد";
                return View("ShowMessage");
            }
        }
        
        return View(viewModel);
    }
    
    #endregion

    #region ForgotPassword
    
    [Route("forgot-password")]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [Route("forgot-password")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        ForgotPasswordResult result = await userService.GetConfirmationCodeForUserAsync(viewModel);

        switch (result)
        {
            case ForgotPasswordResult.Success:
                
                return RedirectToAction(nameof(ResetPassword));
            case ForgotPasswordResult.UserNotFound:
                ModelState.AddModelError("Email", "هیچ کاربری با این ایمیل پیدا نشد");
                return View(viewModel);
            case ForgotPasswordResult.EmailNotFound:
                ModelState.AddModelError("Email", "ایمیل یافت نشد");
                return View(viewModel);
            default:
                return View(viewModel);
        }
    }

    #endregion

    #region Logout
    
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

    #endregion

    #region ResetPassword
    
    [Route("reset-password")]
    public IActionResult ResetPassword()
    {
        return View();
    }

    [Route("reset-password")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        ResetPasswordResult result = await userService.ResetPasswordAsync(viewModel);

        if (result == ResetPasswordResult.WrongCode)
        {
            ModelState.AddModelError("ConfirmCode", "کد تایید اشتباه میباشد");
            return View(viewModel);
        }

        return RedirectToAction(nameof(Login));
    }

    #endregion
}
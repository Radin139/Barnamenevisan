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

    #region Logout
    
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

    #endregion
}
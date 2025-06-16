using Barnamenevisan.Core.Services.Interfaces;
using Barnamenevisan.Domain.ViewModels.Admin.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barnamenevisan.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class UserController(IUserService userService) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var list = await userService.GetUsersForAdminAsync();
        return View(list);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        
        CreateUserResult result = await userService.CreateUserAsync(viewModel);
        switch (result)
        {
            case CreateUserResult.UsernameExists:
                ModelState.AddModelError("Username", "نام کاربری موجود میباشذ");
                return View(viewModel);
            case CreateUserResult.Success:
                return RedirectToAction("Index");
            default:
                return View(viewModel);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        bool result = await userService.DeleteUserAsync(id);
        if (result)
        {
            return RedirectToAction("Index");
        }
        
        return NotFound();
    }

    public async Task<IActionResult> Restore(int id)
    {
        bool result = await userService.RestoreUserAsync(id);
        if (result)
        {
            return RedirectToAction("Index");
        }
        
        return NotFound();
    }
    
    
    public async Task<IActionResult> Edit(int id)
    {
        UserEditViewModel? viewModel = await userService.GetUserForEditAsync(id);
        if (viewModel == null)
        {
            return NotFound();
        }
        
        return View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserEditViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        
        bool result = await userService.EditUserAsync(viewModel);

        if (!result)
        {
            return View(viewModel);
        }
        
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeletePermanently(int id)
    {
        UserDeletePermanentlyViewModel? viewModel = await userService.GetUserForPermanentlyDeleteAsync(id);

        if (viewModel == null)
        {
            return NotFound();
        }
        
        return View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePermanently(UserDeletePermanentlyViewModel viewModel)
    {
        bool result = await userService.DeleteUserPermanentlyAsync(viewModel.Id);

        if (!result)
        {
            return View(viewModel);
        }
        return RedirectToAction("Index");
    }
}
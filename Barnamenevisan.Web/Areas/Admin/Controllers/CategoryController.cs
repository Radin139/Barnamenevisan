using Barnamenevisan.Core.Services.Interfaces;
using Barnamenevisan.Domain.Models.Ecommerce;
using Barnamenevisan.Domain.ViewModels.Admin.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barnamenevisan.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class CategoryController(ICategoryService categoryService) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var list = await categoryService.GetCategoriesForAdminAsync();
        return View(list);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        
        await categoryService.CreateCategoryAsync(viewModel);
        
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        CategoryEditViewModel? viewModel = await categoryService.GetCategoryForEditAsync(id);

        if (viewModel == null)
        {
            return NotFound();
        }
        
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryEditViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        
        bool result = await categoryService.EditCategoryAsync(viewModel);

        if (!result)
        {
            return View(viewModel);
        }
        
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        bool result = await categoryService.DeleteCategoryAsync(id);
        if (result)
        {
            return RedirectToAction("Index");
        }
        
        return NotFound();
    }
    
    public async Task<IActionResult> Restore(int id)
    {
        bool result = await categoryService.RestoreCategoryAsync(id);
        if (result)
        {
            return RedirectToAction("Index");
        }
        
        return NotFound();
    }
    
    public async Task<IActionResult> DeletePermanently(int id)
    {
        CategoryDeletePermanentlyViewModel? viewModel = await categoryService.GetCategoryForPermanentlyDeleteAsync(id);

        if (viewModel == null)
        {
            return NotFound();
        }
        
        return View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePermanently(CategoryDeletePermanentlyViewModel viewModel)
    {
        bool result = await categoryService.DeleteCategoryPermanentlyAsync(viewModel.Id);

        if (!result)
        {
            return View(viewModel);
        }
        
        return RedirectToAction("Index");
    }
}
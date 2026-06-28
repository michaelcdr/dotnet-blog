using CodingBlog.Models;
using CodingBlog.Models.Admin;
using CodingBlog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class CategoriesController : Controller
{
    private readonly IBlogApiService _blogApiService;

    public CategoriesController(IBlogApiService blogApiService)
    {
        _blogApiService = blogApiService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Grid([FromQuery] AdminGridRequest request, CancellationToken cancellationToken)
    {
        var result = await _blogApiService.ObterCategoriasAdmin(request, cancellationToken);
        return Json(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View("Form", new AdminCategoryFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdminCategoryFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ValidateCategory(model))
            return View("Form", model);

        try
        {
            await _blogApiService.CriarCategoriaAdmin(model, cancellationToken);
            TempData["AdminSuccess"] = "Categoria cadastrada com sucesso.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiValidationException ex)
        {
            AddErrors(ex.Errors);
            return View("Form", model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var category = await _blogApiService.ObterCategoriaPorId(id);

        return View("Form", new AdminCategoryFormViewModel
        {
            Id = category.CategoriaId == 0 ? id : category.CategoriaId,
            Nome = category.Nome
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AdminCategoryFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ValidateCategory(model))
            return View("Form", model);

        try
        {
            await _blogApiService.AtualizarCategoriaAdmin(model, cancellationToken);
            TempData["AdminSuccess"] = "Categoria atualizada com sucesso.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiValidationException ex)
        {
            AddErrors(ex.Errors);
            return View("Form", model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var selected = await _blogApiService.ObterCategoriaPorId(id);

        var options = await _blogApiService.ObterOpcoesCategorias(cancellationToken);

        return View(new AdminCategoryDeleteViewModel
        {
            Id = selected.Id,
            Nome = selected.Nome,
            QtdPosts = selected.QtdPosts,
            AvailableCategories = options.Where(item => item.Id != id).ToArray()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(AdminCategoryDeleteViewModel model, CancellationToken cancellationToken)
    {
        model.AvailableCategories = (await _blogApiService.ObterOpcoesCategorias(cancellationToken))
            .Where(item => item.Id != model.Id)
            .ToArray();

        if (model.QtdPosts > 0 && !model.DestinationCategoryId.HasValue && string.IsNullOrWhiteSpace(model.NewCategoryName))
        {
            ModelState.AddModelError(string.Empty, "Escolha uma categoria de destino ou crie uma nova categoria antes de excluir.");
            return View(model);
        }

        try
        {
            if (!string.IsNullOrWhiteSpace(model.NewCategoryName))
            {
                var newCategory = new AdminCategoryFormViewModel { Nome = model.NewCategoryName.Trim() };
                await _blogApiService.CriarCategoriaAdmin(newCategory, cancellationToken);

                var categories = await _blogApiService.ObterOpcoesCategorias(cancellationToken);
                model.DestinationCategoryId = categories
                    .Where(item => string.Equals(item.Nome, newCategory.Nome, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(item => item.Id)
                    .Select(item => (int?)item.Id)
                    .FirstOrDefault();
            }

            await _blogApiService.RemoverCategoriaAdmin(model.Id, model.DestinationCategoryId, cancellationToken);
            TempData["AdminSuccess"] = "Categoria removida com sucesso.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiValidationException ex)
        {
            AddErrors(ex.Errors);
            return View(model);
        }
    }

    private bool ValidateCategory(AdminCategoryFormViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Nome))
            ModelState.AddModelError(nameof(model.Nome), "Informe o nome da categoria.");

        return ModelState.IsValid;
    }

    private void AddErrors(IEnumerable<string> errors)
    {
        foreach (var error in errors)
            ModelState.AddModelError(string.Empty, error);
    }
}

using CodingBlog.Models.Admin;
using CodingBlog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class PostsController : Controller
{
    private readonly IBlogApiService _blogApiService;
    private readonly ILocalArticleImageStorage _imageStorage;

    public PostsController(IBlogApiService blogApiService, ILocalArticleImageStorage imageStorage)
    {
        _blogApiService = blogApiService;
        _imageStorage = imageStorage;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Categories = await _blogApiService.ObterOpcoesCategorias(cancellationToken);
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Grid([FromQuery] AdminGridRequest request, CancellationToken cancellationToken)
    {
        var result = await _blogApiService.ObterPostsAdmin(request, cancellationToken);
        return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        return View("Form", await BuildPostForm(cancellationToken));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdminPostFormViewModel model, CancellationToken cancellationToken)
    {
        await PopulateCategories(model, cancellationToken);

        if (!ValidatePost(model))
            return View("Form", model);

        try
        {
            model.Imagem = await ResolveImage(model, cancellationToken);
            await _blogApiService.CriarPostAdmin(model, cancellationToken);
            TempData["AdminSuccess"] = "Post cadastrado com sucesso.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiValidationException ex)
        {
            AddErrors(ex.Errors);
            return View("Form", model);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(nameof(model.ImageUpload), ex.Message);
            return View("Form", model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var model = await _blogApiService.ObterPostAdminPorId(id, cancellationToken);
        return model == null ? NotFound() : View("Form", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AdminPostFormViewModel model, CancellationToken cancellationToken)
    {
        await PopulateCategories(model, cancellationToken);

        if (!ValidatePost(model))
            return View("Form", model);

        try
        {
            model.Imagem = await ResolveImage(model, cancellationToken);
            await _blogApiService.AtualizarPostAdmin(model, cancellationToken);
            TempData["AdminSuccess"] = "Post atualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiValidationException ex)
        {
            AddErrors(ex.Errors);
            return View("Form", model);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(nameof(model.ImageUpload), ex.Message);
            return View("Form", model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _blogApiService.RemoverPostAdmin(id, cancellationToken);
            TempData["AdminSuccess"] = "Post removido com sucesso.";
        }
        catch (ApiValidationException ex)
        {
            TempData["AdminError"] = string.Join(" ", ex.Errors);
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<AdminPostFormViewModel> BuildPostForm(CancellationToken cancellationToken)
    {
        return new AdminPostFormViewModel
        {
            Categories = await _blogApiService.ObterOpcoesCategorias(cancellationToken)
        };
    }

    private async Task PopulateCategories(AdminPostFormViewModel model, CancellationToken cancellationToken)
    {
        model.Categories = await _blogApiService.ObterOpcoesCategorias(cancellationToken);
    }

    private bool ValidatePost(AdminPostFormViewModel model)
    {
        if (model.CategoriaId <= 0)
            ModelState.AddModelError(nameof(model.CategoriaId), "Selecione uma categoria.");

        if (string.IsNullOrWhiteSpace(model.Titulo))
            ModelState.AddModelError(nameof(model.Titulo), "Informe o titulo do post.");

        if (string.IsNullOrWhiteSpace(model.Descritivo))
            ModelState.AddModelError(nameof(model.Descritivo), "Informe o conteudo do post.");

        return ModelState.IsValid;
    }

    private async Task<string?> ResolveImage(AdminPostFormViewModel model, CancellationToken cancellationToken)
    {
        if (model.ImageUpload == null)
            return model.Imagem;

        return await _imageStorage.SaveAsync(model.ImageUpload, cancellationToken);
    }

    private void AddErrors(IEnumerable<string> errors)
    {
        foreach (var error in errors)
            ModelState.AddModelError(string.Empty, error);
    }
}

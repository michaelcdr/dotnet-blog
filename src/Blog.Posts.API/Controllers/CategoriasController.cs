using Blog.Core.Controller;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using AdminListCategories = Blog.Posts.API.Features.Categories.AdminListCategories;
using CreateCategory = Blog.Posts.API.Features.Categories.CreateCategory;
using DeleteCategory = Blog.Posts.API.Features.Categories.DeleteCategory;
using GetCategoryById = Blog.Posts.API.Features.Categories.GetCategoryById;
using ListCategories = Blog.Posts.API.Features.Categories.ListCategories;
using ListCategoryOptions = Blog.Posts.API.Features.Categories.ListCategoryOptions;
using UpdateCategory = Blog.Posts.API.Features.Categories.UpdateCategory;

namespace Blog.Posts.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : MainApiController
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromServices] ListCategories.Handler handler,
        [FromQuery] ListCategories.Request request,
        CancellationToken cancellationToken)
    {
        var categorias = await handler.Handle(request, cancellationToken);
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(
        [FromServices] GetCategoryById.Handler handler,
        int id,
        CancellationToken cancellationToken)
    {
        var categoria = await handler.Handle(id, cancellationToken);
        return categoria == null ? NotFound() : Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromServices] CreateCategory.Handler handler,
        [FromServices] IValidator<CreateCategory.Request> validator,
        CreateCategory.Request request,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);

        foreach (var error in validation.Errors)
            AddError(error.ErrorMessage);

        if (!validation.IsValid)
            return CustomResponse();

        var handlerError = await handler.Handle(request, cancellationToken);

        if (handlerError != null)
        {
            AddError(handlerError);
            return CustomResponse();
        }

        return CustomResponse();
    }

    [HttpGet("admin")]
    public async Task<IActionResult> GetAdmin(
        [FromServices] AdminListCategories.Handler handler,
        [FromQuery] AdminListCategories.Request request,
        CancellationToken cancellationToken)
    {
        var categorias = await handler.Handle(request, cancellationToken);
        return Ok(categorias);
    }

    [HttpGet("admin/options")]
    public async Task<IActionResult> GetOptions(
        [FromServices] ListCategoryOptions.Handler handler,
        CancellationToken cancellationToken)
    {
        var categorias = await handler.Handle(cancellationToken);
        return Ok(categorias);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(
        [FromServices] UpdateCategory.Handler handler,
        [FromServices] IValidator<UpdateCategory.Request> validator,
        int id,
        UpdateCategory.Request request,
        CancellationToken cancellationToken)
    {
        request.Id = id;

        var validation = await validator.ValidateAsync(request, cancellationToken);

        foreach (var error in validation.Errors)
            AddError(error.ErrorMessage);

        if (!validation.IsValid)
            return CustomResponse();

        var handlerError = await handler.Handle(request, cancellationToken);

        if (handlerError != null)
        {
            AddError(handlerError);
            return CustomResponse();
        }

        return CustomResponse();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        [FromServices] DeleteCategory.Handler handler,
        [FromServices] IValidator<DeleteCategory.Request> validator,
        int id,
        [FromQuery] int? destinationCategoryId,
        CancellationToken cancellationToken)
    {
        var request = new DeleteCategory.Request
        {
            Id = id,
            DestinationCategoryId = destinationCategoryId
        };

        var validation = await validator.ValidateAsync(request, cancellationToken);

        foreach (var error in validation.Errors)
            AddError(error.ErrorMessage);

        if (!validation.IsValid)
            return CustomResponse();

        var handlerError = await handler.Handle(request, cancellationToken);

        if (handlerError != null)
        {
            AddError(handlerError);
            return CustomResponse();
        }

        return CustomResponse();
    }
}
using Blog.Core.Controller;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using AdminListPosts = Blog.Posts.API.Features.Posts.AdminListPosts;
using CreateAdminPost = Blog.Posts.API.Features.Posts.CreateAdminPost;
using CreatePost = Blog.Posts.API.Features.Posts.CreatePost;
using DeletePost = Blog.Posts.API.Features.Posts.DeletePost;
using GetAdminPostById = Blog.Posts.API.Features.Posts.GetAdminPostById;
using GetPostById = Blog.Posts.API.Features.Posts.GetPostById;
using ListPosts = Blog.Posts.API.Features.Posts.ListPosts;
using ListPostsByCategory = Blog.Posts.API.Features.Posts.ListPostsByCategory;
using ListPostsByTag = Blog.Posts.API.Features.Posts.ListPostsByTag;
using ListRecentPosts = Blog.Posts.API.Features.Posts.ListRecentPosts;
using ListTags = Blog.Posts.API.Features.Posts.ListTags;
using SearchPosts = Blog.Posts.API.Features.Posts.SearchPosts;
using UpdateAdminPost = Blog.Posts.API.Features.Posts.UpdateAdminPost;
using UpdatePost = Blog.Posts.API.Features.Posts.UpdatePost;

namespace Blog.Posts.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostsController : MainApiController
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromServices] ListPosts.Handler handler,
        [FromQuery] ListPosts.Request request,
        CancellationToken cancellationToken)
    {
        var posts = await handler.Handle(request, cancellationToken);
        return Ok(posts);
    }

    [HttpGet("pesquisa/{pesquisa?}")]
    public async Task<IActionResult> Pesquisa(
        [FromServices] SearchPosts.Handler handler,
        [FromRoute] string? pesquisa,
        [FromQuery] SearchPosts.Request request,
        CancellationToken cancellationToken)
    {
        request.Pesquisa = pesquisa;
        var posts = await handler.Handle(request, cancellationToken);
        return Ok(posts);
    }

    [HttpGet("por-tag/{tag?}")]
    public async Task<IActionResult> ObterPorTag(
        [FromServices] ListPostsByTag.Handler handler,
        [FromRoute] string? tag,
        [FromQuery] ListPostsByTag.Request request,
        CancellationToken cancellationToken)
    {
        request.Tag = tag;
        var posts = await handler.Handle(request, cancellationToken);
        return Ok(posts);
    }

    [HttpGet("por-categoria/{id}")]
    public async Task<IActionResult> ObterPorCategoria(
        [FromServices] ListPostsByCategory.Handler handler,
        [FromRoute] int id,
        [FromQuery] ListPostsByCategory.Request request,
        CancellationToken cancellationToken)
    {
        request.CategoriaId = id;
        var posts = await handler.Handle(request, cancellationToken);
        return Ok(posts);
    }

    [HttpGet("recentes")]
    public async Task<IActionResult> ObterRecentes(
        [FromServices] ListRecentPosts.Handler handler,
        CancellationToken cancellationToken)
    {
        var posts = await handler.Handle(cancellationToken);
        return Ok(posts);
    }

    [HttpGet("tags")]
    public async Task<IActionResult> ObterTags(
        [FromServices] ListTags.Handler handler,
        CancellationToken cancellationToken)
    {
        var tags = await handler.Handle(cancellationToken);
        return Ok(tags);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPorId(
        [FromServices] GetPostById.Handler handler,
        int id,
        CancellationToken cancellationToken)
    {
        var post = await handler.Handle(id, cancellationToken);
        return post == null ? NotFound() : Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromServices] CreatePost.Handler handler,
        [FromServices] IValidator<CreatePost.Request> validator,
        CreatePost.Request request,
        CancellationToken cancellationToken)
    {
        if (!await Validate(request, validator, cancellationToken))
            return CustomResponse();

        var (response, error) = await handler.Handle(request, cancellationToken);

        if (error != null)
        {
            AddError(error);
            return CustomResponse();
        }

        return CreatedAtAction(nameof(GetPorId), new { id = response!.Id }, response);
    }

    [HttpPut]
    public async Task<IActionResult> Put(
        [FromServices] UpdatePost.Handler handler,
        [FromServices] IValidator<UpdatePost.Request> validator,
        UpdatePost.Request request,
        CancellationToken cancellationToken)
    {
        if (!await Validate(request, validator, cancellationToken))
            return CustomResponse();

        var error = await handler.Handle(request, cancellationToken);

        if (error != null)
        {
            AddError(error);
            return CustomResponse();
        }

        return CustomResponse();
    }

    [HttpGet("admin")]
    public async Task<IActionResult> GetAdmin(
        [FromServices] AdminListPosts.Handler handler,
        [FromQuery] AdminListPosts.Request request,
        CancellationToken cancellationToken)
    {
        var posts = await handler.Handle(request, cancellationToken);
        return Ok(posts);
    }

    [HttpGet("admin/{id}")]
    public async Task<IActionResult> GetAdminById(
        [FromServices] GetAdminPostById.Handler handler,
        int id,
        CancellationToken cancellationToken)
    {
        var post = await handler.Handle(id, cancellationToken);
        return post == null ? NotFound() : Ok(post);
    }

    [HttpPost("admin")]
    public async Task<IActionResult> PostAdmin(
        [FromServices] CreateAdminPost.Handler handler,
        [FromServices] IValidator<CreateAdminPost.Request> validator,
        CreateAdminPost.Request request,
        CancellationToken cancellationToken)
    {
        if (!await Validate(request, validator, cancellationToken))
            return CustomResponse();

        var (id, error) = await handler.Handle(request, cancellationToken);

        if (error != null)
        {
            AddError(error);
            return CustomResponse();
        }

        return CreatedAtAction(nameof(GetAdminById), new { id }, new { id });
    }

    [HttpPut("admin/{id}")]
    public async Task<IActionResult> PutAdmin(
        [FromServices] UpdateAdminPost.Handler handler,
        [FromServices] IValidator<UpdateAdminPost.Request> validator,
        int id,
        UpdateAdminPost.Request request,
        CancellationToken cancellationToken)
    {
        request.Id = id;

        if (!await Validate(request, validator, cancellationToken))
            return CustomResponse();

        var error = await handler.Handle(request, cancellationToken);

        if (error != null)
        {
            AddError(error);
            return CustomResponse();
        }

        return CustomResponse();
    }

    [HttpDelete("admin/{id}")]
    public async Task<IActionResult> DeleteAdmin(
        [FromServices] DeletePost.Handler handler,
        int id,
        CancellationToken cancellationToken)
    {
        var error = await handler.Handle(id, cancellationToken);

        if (error != null)
        {
            AddError(error);
            return CustomResponse();
        }

        return CustomResponse();
    }

    private async Task<bool> Validate<TRequest>(
        TRequest request,
        IValidator<TRequest> validator,
        CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(request, cancellationToken);

        foreach (var error in result.Errors)
            AddError(error.ErrorMessage);

        return result.IsValid;
    }
}
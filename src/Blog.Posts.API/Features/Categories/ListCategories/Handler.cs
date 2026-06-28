using Blog.Core.Models;
using Blog.Posts.API.Features.Categories.Shared;
using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Categories.ListCategories;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResponse<CategoryResponse>> Handle(Request request, CancellationToken cancellationToken)
    {
        var query = _db.Categorias.AsNoTracking().OrderBy(categoria => categoria.Nome);
        var totalItems = await query.CountAsync(cancellationToken);
        var categories = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(categoria => new CategoryResponse
            {
                Id = categoria.Id,
                Nome = categoria.Nome ?? string.Empty,
                QtdPosts = categoria.QtdPosts
            })
            .ToListAsync(cancellationToken);

        return PagedResponse<CategoryResponse>.Create(categories, request.Page, request.PageSize, totalItems);
    }
}

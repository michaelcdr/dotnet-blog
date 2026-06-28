using Blog.Core.Models;
using Blog.Posts.API.Features.Categories.Shared;
using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Categories.AdminListCategories;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResponse<CategoryResponse>> Handle(Request request, CancellationToken cancellationToken)
    {
        var query = _db.Categorias.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(categoria => (categoria.Nome ?? string.Empty).Contains(search));
        }

        query = (request.SortBy?.ToLowerInvariant(), request.SortDirection?.ToLowerInvariant()) switch
        {
            ("qtdposts", "desc") => query.OrderByDescending(categoria => categoria.QtdPosts).ThenBy(categoria => categoria.Nome),
            ("qtdposts", _) => query.OrderBy(categoria => categoria.QtdPosts).ThenBy(categoria => categoria.Nome),
            ("id", "desc") => query.OrderByDescending(categoria => categoria.Id),
            ("id", _) => query.OrderBy(categoria => categoria.Id),
            ("nome", "desc") => query.OrderByDescending(categoria => categoria.Nome),
            _ => query.OrderBy(categoria => categoria.Nome)
        };

        var totalItems = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(categoria => new CategoryResponse
            {
                Id = categoria.Id,
                Nome = categoria.Nome ?? string.Empty,
                QtdPosts = categoria.QtdPosts
            })
            .ToListAsync(cancellationToken);

        return PagedResponse<CategoryResponse>.Create(items, request.Page, request.PageSize, totalItems);
    }
}

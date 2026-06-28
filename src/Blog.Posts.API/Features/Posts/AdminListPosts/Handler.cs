using Blog.Core.Models;
using Blog.Posts.API.Features.Posts.Shared;
using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Posts.AdminListPosts;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResponse<PostResponse>> Handle(Request request, CancellationToken cancellationToken)
    {
        var query = _db.Posts.AsNoTracking();

        if (request.CategoryId.HasValue)
            query = query.Where(post => post.CategoriaId == request.CategoryId.Value);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(post =>
                post.Titulo.Contains(search) ||
                post.Descritivo.Contains(search) ||
                post.Tags.Contains(search));
        }

        query = (request.SortBy?.ToLowerInvariant(), request.SortDirection?.ToLowerInvariant()) switch
        {
            ("titulo", "asc") => query.OrderBy(post => post.Titulo),
            ("titulo", "desc") => query.OrderByDescending(post => post.Titulo),
            ("categoria", "asc") => query.OrderBy(post => post.Categoria.Nome).ThenByDescending(post => post.Id),
            ("categoria", "desc") => query.OrderByDescending(post => post.Categoria.Nome).ThenByDescending(post => post.Id),
            ("tags", "asc") => query.OrderBy(post => post.Tags).ThenByDescending(post => post.Id),
            ("tags", "desc") => query.OrderByDescending(post => post.Tags).ThenByDescending(post => post.Id),
            ("id", "asc") => query.OrderBy(post => post.Id),
            _ => query.OrderByDescending(post => post.Id)
        };

        var totalItems = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(PostProjections.ToResponse)
            .ToListAsync(cancellationToken);

        return PagedResponse<PostResponse>.Create(items, request.Page, request.PageSize, totalItems);
    }
}

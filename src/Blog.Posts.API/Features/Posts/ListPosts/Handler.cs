using Blog.Core.Models;
using Blog.Posts.API.Features.Posts.Shared;
using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Posts.ListPosts;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResponse<PostResponse>> Handle(Request request, CancellationToken cancellationToken)
    {
        var query = _db.Posts.AsNoTracking().OrderByDescending(post => post.Id);
        var totalItems = await query.CountAsync(cancellationToken);
        var posts = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(PostProjections.ToResponse)
            .ToListAsync(cancellationToken);

        return PagedResponse<PostResponse>.Create(posts, request.Page, request.PageSize, totalItems);
    }
}

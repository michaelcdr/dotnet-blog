using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Posts.ListRecentPosts;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Response>> Handle(CancellationToken cancellationToken)
    {
        return await _db.Posts
            .AsNoTracking()
            .OrderByDescending(post => post.Id)
            .Take(5)
            .Select(post => new Response { Id = post.Id, Titulo = post.Titulo })
            .ToListAsync(cancellationToken);
    }
}

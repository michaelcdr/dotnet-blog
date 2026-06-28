using Blog.Posts.API.Features.Posts.Shared;
using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Posts.GetPostById;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PostResponse?> Handle(int id, CancellationToken cancellationToken)
    {
        return await _db.Posts
            .AsNoTracking()
            .Where(post => post.Id == id)
            .Select(PostProjections.ToResponse)
            .SingleOrDefaultAsync(cancellationToken);
    }
}

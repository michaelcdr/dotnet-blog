using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Posts.ListTags;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<string>> Handle(CancellationToken cancellationToken)
    {
        var tags = await _db.Posts
            .AsNoTracking()
            .Where(post => !string.IsNullOrEmpty(post.Tags))
            .Select(post => post.Tags)
            .ToListAsync(cancellationToken);

        return tags
            .SelectMany(tag => tag.Split(","))
            .Select(tag => tag.Trim())
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Distinct()
            .OrderBy(tag => tag)
            .ToList();
    }
}

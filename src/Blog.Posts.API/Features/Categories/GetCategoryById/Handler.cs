using Blog.Posts.API.Features.Categories.Shared;
using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Categories.GetCategoryById;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<CategoryResponse?> Handle(int id, CancellationToken cancellationToken)
    {
        return await _db.Categorias
            .AsNoTracking()
            .Where(categoria => categoria.Id == id)
            .Select(categoria => new CategoryResponse
            {
                Id = categoria.Id,
                Nome = categoria.Nome ?? string.Empty,
                QtdPosts = categoria.QtdPosts
            })
            .SingleOrDefaultAsync(cancellationToken);
    }
}

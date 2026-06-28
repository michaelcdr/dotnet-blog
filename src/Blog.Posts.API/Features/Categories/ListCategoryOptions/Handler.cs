using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Categories.ListCategoryOptions;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyCollection<Response>> Handle(CancellationToken cancellationToken)
    {
        return await _db.Categorias
            .AsNoTracking()
            .OrderBy(categoria => categoria.Nome)
            .Select(categoria => new Response
            {
                Id = categoria.Id,
                Nome = categoria.Nome ?? string.Empty
            })
            .ToListAsync(cancellationToken);
    }
}

using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Categories.UpdateCategory;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<string?> Handle(Request request, CancellationToken cancellationToken)
    {
        var category = await _db.Categorias.SingleOrDefaultAsync(categoria => categoria.Id == request.Id, cancellationToken);

        if (category == null)
            return $"Nao foi encontrada uma categoria com id {request.Id}.";

        if (await _db.Categorias.AnyAsync(
                categoria => categoria.Id != request.Id && categoria.Nome == request.Nome,
                cancellationToken))
        {
            return $"Ja existe uma categoria chamada {request.Nome}.";
        }

        category.Nome = request.Nome;
        await _db.SaveChangesAsync(cancellationToken);

        return null;
    }
}

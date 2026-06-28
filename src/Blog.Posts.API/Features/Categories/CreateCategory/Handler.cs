using Blog.Posts.Data.Contexts.SqlServer;
using Blog.Posts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Categories.CreateCategory;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<string?> Handle(Request request, CancellationToken cancellationToken)
    {
        if (await _db.Categorias.AnyAsync(categoria => categoria.Nome == request.Nome, cancellationToken))
            return $"Ja existe uma categoria chamada {request.Nome}.";

        _db.Categorias.Add(new Categoria(0, request.Nome));
        await _db.SaveChangesAsync(cancellationToken);

        return null;
    }
}

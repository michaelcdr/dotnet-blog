using Blog.Posts.Data.Contexts.SqlServer;
using Blog.Posts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Posts.CreateAdminPost;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(int? Id, string? Error)> Handle(Request request, CancellationToken cancellationToken)
    {
        var category = await _db.Categorias
            .SingleOrDefaultAsync(categoria => categoria.Id == request.CategoriaId, cancellationToken);

        if (category == null)
            return (null, $"A categoria com id {request.CategoriaId} nao foi encontrada.");

        var tags = request.Tags
            .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var post = new Post(
            0,
            request.Titulo,
            request.Descritivo,
            request.Imagem ?? string.Empty,
            "admin",
            string.Join(",", tags),
            category.Id);

        _db.Posts.Add(post);
        category.IncrementarQtdPosts();
        await _db.SaveChangesAsync(cancellationToken);

        return (post.Id, null);
    }
}

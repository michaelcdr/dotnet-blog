using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Posts.UpdateAdminPost;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<string?> Handle(Request request, CancellationToken cancellationToken)
    {
        var category = await _db.Categorias
            .SingleOrDefaultAsync(categoria => categoria.Id == request.CategoriaId, cancellationToken);

        if (category == null)
            return $"A categoria com id {request.CategoriaId} nao foi encontrada.";

        var post = await _db.Posts.SingleOrDefaultAsync(currentPost => currentPost.Id == request.Id, cancellationToken);

        if (post == null)
            return $"Nao foi encontrado um post com id {request.Id}.";

        if (post.CategoriaId != request.CategoriaId)
        {
            var oldCategory = await _db.Categorias.SingleOrDefaultAsync(categoria => categoria.Id == post.CategoriaId, cancellationToken);
            oldCategory?.DecrementarQtdPosts();
            category.IncrementarQtdPosts();
        }

        post.Atualizar(
            request.Titulo,
            request.Descritivo,
            request.Imagem ?? string.Empty,
            request.Tags,
            request.CategoriaId,
            "admin");

        await _db.SaveChangesAsync(cancellationToken);

        return null;
    }
}

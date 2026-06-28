using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Posts.UpdatePost;

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
            .SingleOrDefaultAsync(categoria => categoria.Nome == request.Categoria, cancellationToken);

        if (category == null)
            return $"A categoria {request.Categoria} nao foi encontrada.";

        var post = await _db.Posts.SingleOrDefaultAsync(post => post.Id == request.Id, cancellationToken);

        if (post == null)
            return $"Nao foi encontrado um post com id {request.Id}.";

        if (post.CategoriaId != category.Id)
        {
            var oldCategory = await _db.Categorias.SingleOrDefaultAsync(categoria => categoria.Id == post.CategoriaId, cancellationToken);
            oldCategory?.DecrementarQtdPosts();
            category.IncrementarQtdPosts();
        }

        post.Atualizar(request.Titulo, request.Descritivo, request.Imagem, request.Tags, category.Id, string.Empty);
        await _db.SaveChangesAsync(cancellationToken);

        return null;
    }
}

using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Categories.DeleteCategory;

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
            .SingleOrDefaultAsync(categoria => categoria.Id == request.Id, cancellationToken);

        if (category == null)
            return $"Nao foi encontrada uma categoria com id {request.Id}.";

        var postsToMove = await _db.Posts
            .Where(post => post.CategoriaId == request.Id)
            .ToListAsync(cancellationToken);

        if (postsToMove.Count > 0)
        {
            if (!request.DestinationCategoryId.HasValue)
                return "Escolha uma categoria de destino para os posts antes de remover esta categoria.";

            var destinationCategory = await _db.Categorias
                .SingleOrDefaultAsync(categoria => categoria.Id == request.DestinationCategoryId.Value, cancellationToken);

            if (destinationCategory == null)
                return "A categoria de destino nao foi encontrada.";

            foreach (var post in postsToMove)
            {
                post.Atualizar(post.Titulo, post.Descritivo, post.Imagem, post.Tags, destinationCategory.Id, post.AlteradoPor);
            }

            destinationCategory.QtdPosts += postsToMove.Count;
        }

        _db.Categorias.Remove(category);
        await _db.SaveChangesAsync(cancellationToken);

        return null;
    }
}

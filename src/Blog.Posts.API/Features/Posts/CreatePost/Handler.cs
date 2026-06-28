using Blog.Posts.Data.Contexts.SqlServer;
using Blog.Posts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Posts.CreatePost;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(Response? Response, string? Error)> Handle(Request request, CancellationToken cancellationToken)
    {
        var category = await _db.Categorias
            .SingleOrDefaultAsync(categoria => categoria.Nome == request.Categoria, cancellationToken);

        if (category == null)
            return (null, $"A categoria {request.Categoria} nao foi encontrada.");

        var tags = request.Tags
            .Split(",")
            .Select(tag => tag.Trim())
            .Where(tag => !string.IsNullOrWhiteSpace(tag));

        var post = new Post(
            0,
            request.Titulo,
            request.Descritivo,
            request.Imagem,
            "michael",
            string.Join(",", tags),
            category.Id);

        _db.Posts.Add(post);
        category.IncrementarQtdPosts();
        await _db.SaveChangesAsync(cancellationToken);

        return (new Response { Id = post.Id }, null);
    }
}

using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Posts.GetAdminPostById;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Response?> Handle(int id, CancellationToken cancellationToken)
    {
        return await _db.Posts
            .AsNoTracking()
            .Where(post => post.Id == id)
            .Select(post => new Response
            {
                Id = post.Id,
                Titulo = post.Titulo,
                Descritivo = post.Descritivo,
                Tags = post.Tags,
                Imagem = post.Imagem,
                CategoriaId = post.CategoriaId
            })
            .SingleOrDefaultAsync(cancellationToken);
    }
}

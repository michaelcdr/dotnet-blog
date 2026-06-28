using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Features.Posts.DeletePost;

public class Handler
{
    private readonly AppDbContext _db;

    public Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<string?> Handle(int id, CancellationToken cancellationToken)
    {
        var post = await _db.Posts.SingleOrDefaultAsync(currentPost => currentPost.Id == id, cancellationToken);

        if (post == null)
            return $"Nao foi encontrado um post com id {id}.";

        var category = await _db.Categorias.SingleOrDefaultAsync(currentCategory => currentCategory.Id == post.CategoriaId, cancellationToken);
        category?.DecrementarQtdPosts();

        _db.Posts.Remove(post);
        await _db.SaveChangesAsync(cancellationToken);

        return null;
    }
}

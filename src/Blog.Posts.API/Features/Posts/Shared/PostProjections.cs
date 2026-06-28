using Blog.Posts.Domain;
using System.Linq.Expressions;

namespace Blog.Posts.API.Features.Posts.Shared;

public static class PostProjections
{
    public static readonly Expression<Func<Post, PostResponse>> ToResponse = post => new PostResponse
    {
        Id = post.Id,
        Titulo = post.Titulo,
        Descritivo = post.Descritivo,
        Imagem = post.Imagem,
        Tags = post.Tags,
        Categoria = post.Categoria.Nome ?? string.Empty,
        CategoriaId = post.CategoriaId
    };
}

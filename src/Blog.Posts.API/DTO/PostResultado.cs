using Blog.Posts.Domain;
using Microsoft.CodeAnalysis.Operations;

namespace Blog.Posts.API.DTO;

public class PostResultado
{
    public int Id { get; set; }
    public string Categoria { get; set; } =string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descritivo { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string Imagem { get; set; } = string.Empty;
    public string DataECriador { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public PostResultado()
    {
        
    }

    public PostResultado(Post post)
    {
        Id = post.Id;
        CategoriaId = post.CategoriaId;
        Categoria = post.Categoria?.Nome ?? string.Empty;
        Titulo = post.Titulo;
        Descritivo = post.Descritivo;
        Tags = post.Tags;
        Imagem = post.Imagem;
    }
}

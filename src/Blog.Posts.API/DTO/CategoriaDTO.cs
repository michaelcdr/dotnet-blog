using System.ComponentModel.DataAnnotations;

namespace Blog.Posts.API.DTO;

public class CategoriaDTO
{
    public int Id { get; set; }
    
    public string? Nome { get; set; }

    public int QtdPosts { get; set; }
}

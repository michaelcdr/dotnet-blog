using System.ComponentModel.DataAnnotations;

namespace Blog.Posts.API.DTOs;

public class CategoriaDTO
{
    public int Id { get; set; }
    
    [Required]
    public string? Nome { get; set; }
}

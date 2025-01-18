using System.ComponentModel.DataAnnotations;

namespace Blog.Posts.API.Requests;

public class CategoryCreate
{
    [Required]
    public string? Nome { get; set; }
}
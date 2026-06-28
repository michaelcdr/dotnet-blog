using Microsoft.AspNetCore.Http;

namespace CodingBlog.Models.Admin;

public class AdminPostFormViewModel
{
    public int Id { get; set; }
    public int CategoriaId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descritivo { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string? Imagem { get; set; }
    public IFormFile? ImageUpload { get; set; }
    public IReadOnlyCollection<CategoryOptionViewModel> Categories { get; set; } = [];
}

using System.ComponentModel.DataAnnotations;

namespace Blog.Posts.API.Requests;

public class PostCreate
{
    [Required(ErrorMessage = "Informe a Categoria")]
    public string Categoria { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o Descritivo")]
    public string Descritivo { get; set; } = string.Empty;

    public string Tags { get; set; }

    public string Imagem { get; set; } = string.Empty;
}

public class PostUpdate
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe a Categoria")]
    public string Categoria { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o Descritivo")]
    public string Descritivo { get; set; } = string.Empty;

    public string Tags { get; set; }

    public string Imagem { get; set; } = string.Empty;
}
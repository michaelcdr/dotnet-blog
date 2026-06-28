namespace CodingBlog.Models;

public class CategoriaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int QtdPosts { get; set; }
}

public class CategoriaCadastroModel
{
    public string Nome { get; set; } = string.Empty;
}

public class CategoriaEdicaoModel
{
    public string Nome { get; set; } = string.Empty;
    public int QtdPosts { get; set; }
    public int Id { get; set; }
    public int CategoriaId
    {
        get => Id;
        set => Id = value;
    }
}

namespace CodingBlog.Models;

public class PostViewModel
{
    public int Id { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descritivo { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string Imagem { get; set; } = string.Empty;
    public string DataECriador { get; set; }

    public int CategoriaId { get; set; }
    public List<string> ObterTags()
    {
        List<string> tagsItens = [];

        if (!string.IsNullOrEmpty(Tags))
        {
            tagsItens = Tags.Split(",").Select(e => e.Trim()).Distinct().OrderBy(e => e).ToList();
        }
        return tagsItens;
    }
}
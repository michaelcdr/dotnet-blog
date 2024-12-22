using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Posts.Domain;

public class Post
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descritivo { get; set; }
    public string? Imagem { get; set; }
    public string? CriadoPor { get; set; }
    public string? Tags { get; set; }
    public Categoria Categoria { get; set; }
    public int CategoriaId { get; set; }
    public DateTime CadastradoEm{ get; set; }

    [NotMapped]
    public List<string> TagsItens { get; set; }
    
    protected Post()
    {
        Categoria = new Categoria(0,string.Empty);
        TagsItens = new List<string>();
    }

    public Post(int id, string titulo, string descritivo, 
                string imagem, string criadoPor, string tags,
                int categoriaId, DateTime data)
    {
        Id = id;
        Titulo = titulo;
        Descritivo = descritivo;
        Imagem = imagem;
        CriadoPor = criadoPor;
        Tags = tags;
        CategoriaId = categoriaId;
        CadastradoEm = data;
        TagsItens = tags.Split(',').ToList();
        Categoria = new Categoria(categoriaId, string.Empty);
    }
    
    
    public string ObterDataECriador() => $"Criado por <strong>{this.CriadoPor}</strong> em <strong>{CadastradoEm.ToString("dd.MM.yyyy")}</strong>";
    public string ObterPedacoDescritivo()
    {
        if (string.IsNullOrEmpty(this.Descritivo)) return string.Empty;

        if (this.Descritivo.Length >= 250)
            return this.Descritivo.Substring(0,250) + "...";

        return this.Descritivo;
    }
}

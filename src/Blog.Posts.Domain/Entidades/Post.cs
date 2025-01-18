using Blog.Core.DomainObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Posts.Domain;

public class Post : IAggregateRoot
{
    public int Id { get; set; }
    public string Titulo { get; private set; } = string.Empty;
    public string Descritivo { get; private set; } = string.Empty;
    public string Imagem { get; private set; } = string.Empty;
    public string CriadoPor { get; private set; } = string.Empty;
    public string Tags { get; private set; } = string.Empty;
    public Categoria Categoria { get; set; }
    public int CategoriaId { get; private set; }
    public DateTime CadastradoEm{ get; private set; }
    public string AlteradoPor { get; private set; }
    public DateTime DataUltimaAlteracao { get; private set; }

    [NotMapped]
    public List<string> TagsItens { get; private set; }
    
    protected Post()
    {
        TagsItens = new List<string>();
    }

    public Post(int id, string titulo, string descritivo, 
                string imagem, string criadoPor, string tags,
                int categoriaId, DateTime? data = null)
    {
        Id = id;
        Titulo = titulo;
        Descritivo = descritivo;
        Imagem = imagem;
        CriadoPor = criadoPor;
        Tags = tags;
        CategoriaId = categoriaId;
        CadastradoEm = data ?? DateTime.Now;
        TagsItens = tags.Split(',').ToList();
    }
    
    public string ObterDataECriador() 
        => $"Criado por <strong>{CriadoPor}</strong> em <strong>{CadastradoEm.ToString("dd.MM.yyyy")}</strong>";

    public string ObterPedacoDescritivo()
    {
        if (string.IsNullOrEmpty(Descritivo)) return string.Empty;

        if (Descritivo.Length >= 250)
            return Descritivo.Substring(0,250) + "...";

        return Descritivo;
    }

    public void Atualizar(string titulo, string descritivo, string imagem, string tags, int categoriaId, string usuario)
    {
        Titulo = titulo;
        Descritivo = descritivo;
        Imagem = imagem;
        Tags = tags;
        CategoriaId = categoriaId;
        AlteradoPor = usuario;
    }
}

namespace CodingBlog.Models;

public class PostDetalhesViewModel
{
    public PostDetalhesViewModel(int categoriaId, int postId, string categoriaNome, string tituloPost,
                                 string dataECriadorPost, string descritivoPost, List<string> tags, string imagem)
    {
        CategoriaId = categoriaId;
        PostId = postId;
        CategoriaNome = categoriaNome;
        TituloPost = tituloPost;
        DataECriadorPost = dataECriadorPost;
        DescritivoPost = descritivoPost;
        Tags = tags;
        Imagem = imagem;
    }

    public int CategoriaId { get; set; }
    public int PostId { get; set; }
    public string CategoriaNome { get; set; }
    public string TituloPost { get; set; }
    public string DataECriadorPost { get; set; }
    public string DescritivoPost { get; set; }
    public List<string> Tags { get; set; }
    public string Imagem { get; set; }
}

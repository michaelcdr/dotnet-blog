namespace CodingBlog.Models;

public class PostViewModel
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Imagem { get; set; }
    public List<string> Tags { get; set; }
    public string Descritivo { get; set; }
    public string DataECriador { get; set; }
}
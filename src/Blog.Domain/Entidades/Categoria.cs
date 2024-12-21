namespace Blog.Domain;

public class Categoria
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public List<Post> Posts {get;set;}

    protected Categoria()
    {
        this.Posts = new List<Post>();
    }

    public Categoria(int id, string nome, List<Post>? posts = null)
    {
        Id = id;
        Posts = posts ?? new List<Post>();
        Nome = nome;
    }
}
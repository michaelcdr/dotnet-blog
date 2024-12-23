namespace Blog.Posts.Domain;

public class Categoria
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public List<Post> Posts { get; set; } = [];
    public int QtdPosts { get; set; } = 0;

    protected Categoria()
    {
        Posts = [];
    }

    public Categoria(int id, string nome, List<Post>? posts = null)
    {
        Id = id;
        Posts = posts ?? new List<Post>();
        Nome = nome;
        QtdPosts = posts == null ? 0 : posts.Count;
    }
}
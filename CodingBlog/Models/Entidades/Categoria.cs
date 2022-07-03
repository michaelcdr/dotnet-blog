namespace CodingBlog.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public List<Post> Posts {get;set;}
        public Categoria()
        {
            this.Posts = new List<Post>();
        }

        public Categoria(string nome)
        {
            this.Posts = new List<Post>();
            this.Nome = nome;
        }
        public Categoria(int id, string nome)
        {
            this.Posts = new List<Post>();
            this.Nome = nome;
            this.Id = id;   
        }
        public Categoria(int id, string nome, List<Post> posts)
        {
            this.Posts = posts;
            this.Nome = nome;
            this.Id = id;
        }
    }
}
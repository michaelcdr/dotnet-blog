namespace CodingBlog.Models
{
    public class PostsPorTagViewModel
    {
        public PostsPorTagViewModel(
            List<Post> posts, 
            List<string> tags, 
            List<Categoria> categorias)
        {
            Posts = posts;
            Tags = tags; 
            Categorias = categorias;
        }

        public List<Post> Posts { get; }
        public List<string> Tags { get; } 
        public List<Categoria> Categorias { get; }
    }
} 
namespace CodingBlog.Models
{
    public class PostDetalhesViewModel
    {
        public PostDetalhesViewModel(
            Post post,
            List<string> tags, 
            List<Categoria> categorias)
        {
            Post = post;
            Tags = tags; 
            Categorias = categorias;
        }

        public Post Post { get; }
        public List<string> Tags { get; } 
        public List<Categoria> Categorias { get; }
    }
} 
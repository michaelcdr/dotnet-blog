using CodingBlog.Models;

namespace CodingBlog.Models
{
    public class PostsPorCategoriaViewModel
    {
        public PostsPorCategoriaViewModel(
            List<Post> posts, 
            List<string> tags,
            List<Post> postsRececentes,
            List<Categoria> categorias)
        {
            Posts = posts;
            Tags = tags;
            PostsRececentes = postsRececentes;
            Categorias = categorias;
        }

        public List<Post> Posts { get; }
        public List<string> Tags { get; }
        public List<Post> PostsRececentes { get; }
        public List<Categoria> Categorias { get; }
    }
} 
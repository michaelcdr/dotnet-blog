using CodingBlog.Models;

namespace CodingBlog.Models
{
    public class PostsPorTagViewModel
    {
        public PostsPorTagViewModel(
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

    public class PostDetalhesViewModel
    {
        public PostDetalhesViewModel(
            Post post,
            List<string> tags,
            List<Post> postsRececentes,
            List<Categoria> categorias)
        {
            Post = post;
            Tags = tags;
            PostsRececentes = postsRececentes;
            Categorias = categorias;
        }

        public Post Post { get; }
        public List<string> Tags { get; }
        public List<Post> PostsRececentes { get; }
        public List<Categoria> Categorias { get; }
    }
} 
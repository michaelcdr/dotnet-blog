using CodingBlog.Models;

namespace CodingBlog.Models
{
    public class PostsPorCategoriaViewModel
    {
        public PostsPorCategoriaViewModel(List<PostViewModel> posts,
                                          CategoriaViewModel categoria)
        {
            Posts = posts; 
            Categoria = categoria;
        }

        public List<PostViewModel> Posts { get; }
        public CategoriaViewModel Categoria { get; set; }
    }
} 
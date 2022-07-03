using CodingBlog.Interfaces;
using CodingBlog.Models;
using CodingBlog.Models.Data;

namespace CodingBlog.Repositorios.EmMemoria
{
    public class PostsRepositorio : IPostsRepositorio
    {
        private List<Post> _dados = new();

        public PostsRepositorio(IContexto contexto)
        {
            this._dados = contexto.Posts;
        }  

        public List<Post> ObterRecentes()
        {
            return _dados.OrderByDescending(e => e.CadastradoEm).ToList();
        }

        public List<string> ObterTodasTags()
        {
            var tags = new List<string>();
            List<string> tagsDosPosts = _dados.Where(e => !string.IsNullOrEmpty(e.Tags)).Select(e => e.Tags).ToList();
            foreach (var tagsDoPost in tagsDosPosts)
            {
                var tagsArray = tagsDoPost.Split(",").Select(e => e.Trim()).ToList();
                tags.AddRange(tagsArray);
            }
            tags = tags.Distinct().ToList();

            return tags;
        }

        public List<Post> ObterPorTags(string tag)
        {
            return _dados.Where(e => e.Tags.Contains(tag)).ToList();
        }

        public List<Post> ObterPorCategoria(int id)
        {
            return _dados.Where(e => e.CategoriaId == id).ToList();
        }

        public Post Obter(int id)
        {
            return _dados.Single(e => e.Id == id);
        }

        public List<Post> ObterPorTermoPesquisa(string pesquisa)
        {
            if (string.IsNullOrEmpty(pesquisa)) return new List<Post>();

            pesquisa = pesquisa.Trim().ToLower();

            return _dados.Where(e => e.Titulo.ToLower().Contains(pesquisa) || e.Tags.ToLower().Contains(pesquisa) || e.Descritivo.ToLower().Contains(pesquisa)).ToList();
        }
    }
}
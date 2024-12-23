using Blog.Core.Data;
using Blog.Posts.Data.Contexts.InMemory;
using Blog.Posts.Domain;
using Blog.Posts.Domain.Repositories;

namespace Blog.Posts.Data.Repositorios.InMemory;

public class PostsRepositorio : IPostRepository
{
    private IContexto _context;
    private List<Post> _dadosPosts = new();

    public IUnitOfWork UnitOfWork => throw new NotImplementedException();

    public PostsRepositorio(IContexto contexto)
    {
        _context = contexto;
        _dadosPosts = contexto.Posts;
    }  

    public async Task<List<Post>> ObterRecentes()
    {
        return _dadosPosts.OrderByDescending(e => e.CadastradoEm).ToList();
    }

    public async Task<List<string>> ObterTodasTags()
    {
        var tags = new List<string>();
        List<string> tagsDosPosts = _dadosPosts.Where(e => !string.IsNullOrEmpty(e.Tags)).Select(e => e.Tags).ToList();
        foreach (var tagsDoPost in tagsDosPosts)
        {
            var tagsArray = tagsDoPost.Split(",").Select(e => e.Trim()).ToList();
            tags.AddRange(tagsArray);
        }
        tags = tags.Distinct().ToList();

        return tags;
    }

    public async Task<List<Post>> ObterPorTags(string tag)
    {
        return _dadosPosts.Where(e => e.Tags.Contains(tag)).ToList();
    }

    public async Task<List<Post>> ObterPorCategoria(int id)
    {
        return _dadosPosts.Where(e => e.CategoriaId == id).ToList();
    }

    public async Task<Post> Obter(int id)
    {
        Post post = _dadosPosts.Single(e => e.Id == id);
        Categoria categoria = this._context.Categorias.Single(e => e.Id == post.CategoriaId);
        post.Categoria = categoria;

        return _dadosPosts.Single(e => e.Id == id);
    }

    public async Task<List<Post>> ObterPorTermoPesquisa(string pesquisa)
    {
        if (string.IsNullOrEmpty(pesquisa)) return new List<Post>();

        pesquisa = pesquisa.Trim().ToLower();

        return _dadosPosts.Where(e => e.Titulo.ToLower().Contains(pesquisa) || 
                                      e.Tags.ToLower().Contains(pesquisa) || 
                                      e.Descritivo.ToLower().Contains(pesquisa)).ToList();
    }

    public void Add(Post postCriado)
    {
        _dadosPosts.Add(postCriado);
    }

    public void Dispose()
    {
        
    }
}
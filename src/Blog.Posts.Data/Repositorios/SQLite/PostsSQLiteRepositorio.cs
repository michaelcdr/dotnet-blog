using Blog.Core.Data;
using Blog.Posts.Data.Contexts.SQLite;
using Blog.Posts.Domain;
using Blog.Posts.Domain.Repositories;

namespace Blog.Data.Repositorios.SQLite;

public class PostsSQLiteRepositorio : IPostRepository
{
    private AppDbContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public PostsSQLiteRepositorio(AppDbContext contexto)
    {
        _context = contexto;
    }

    public async Task<List<Post>> ObterRecentes()
    {
        return _context.Posts.OrderByDescending(e => e.CadastradoEm).ToList();
    }

    public async Task<List<string>> ObterTodasTags()
    {
        var tags = new List<string>();
        
        List<string> tagsDosPosts = _context.Posts
            .Where(e => !string.IsNullOrEmpty(e.Tags)).Select(e => e.Tags)
            .ToList();

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
        return _context.Posts.Where(e => e.Tags.Contains(tag)).ToList();
    }

    public async Task<List<Post>> ObterPorCategoria(int id)
    {
        return _context.Posts.Where(e => e.CategoriaId == id).ToList();
    }

    public async Task<Post> Obter(int id)
    {
        Post post = _context.Posts.Single(e => e.Id == id);

        Categoria categoria = _context.Categorias.Single(e => e.Id == post.CategoriaId);

        post.Categoria = categoria;

        return post;
    }

    public async Task<List<Post>> ObterPorTermoPesquisa(string pesquisa)
    {
        if (string.IsNullOrEmpty(pesquisa)) return [];

        pesquisa = pesquisa.Trim().ToLower();

        return _context.Posts
            .Where(e => e.Titulo.ToLower().Contains(pesquisa) ||
                        e.Tags.ToLower().Contains(pesquisa) ||
                        e.Descritivo.ToLower().Contains(pesquisa)).ToList();
    }

    public void Add(Post postCriado)
    {
        _context.Posts.Add(postCriado);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
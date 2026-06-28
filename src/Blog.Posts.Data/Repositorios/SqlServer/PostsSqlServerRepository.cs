using Blog.Core.Data;
using Blog.Posts.Data.Contexts.SqlServer;
using Blog.Posts.Domain;
using Blog.Posts.Domain.Repositories;

namespace Blog.Data.Repositorios.SqlServer;

public class PostsSqlServerRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public PostsSqlServerRepository(AppDbContext contexto)
    {
        _context = contexto;
    }

    public Task<List<Post>> ObterRecentes()
    {
        return Task.FromResult(_context.Posts.OrderByDescending(e => e.CadastradoEm).ToList());
    }

    public Task<List<string>> ObterTodasTags()
    {
        var tags = new List<string>();

        var tagsDosPosts = _context.Posts
            .Where(e => !string.IsNullOrEmpty(e.Tags))
            .Select(e => e.Tags)
            .ToList();

        foreach (var tagsDoPost in tagsDosPosts)
        {
            var tagsArray = tagsDoPost.Split(",").Select(e => e.Trim()).ToList();
            tags.AddRange(tagsArray);
        }

        tags = tags.Distinct().ToList();

        return Task.FromResult(tags);
    }

    public Task<List<Post>> ObterPorTags(string tag)
    {
        return Task.FromResult(_context.Posts.Where(e => e.Tags.Contains(tag)).ToList());
    }

    public Task<List<Post>> ObterPorCategoria(int id)
    {
        return Task.FromResult(_context.Posts.Where(e => e.CategoriaId == id).ToList());
    }

    public Task<Post> Obter(int id)
    {
        var post = _context.Posts.Single(e => e.Id == id);
        var categoria = _context.Categorias.Single(e => e.Id == post.CategoriaId);

        post.Categoria = categoria;

        return Task.FromResult(post);
    }

    public Task<List<Post>> ObterPorTermoPesquisa(string pesquisa)
    {
        if (string.IsNullOrEmpty(pesquisa))
            return Task.FromResult(new List<Post>());

        pesquisa = pesquisa.Trim().ToLower();

        return Task.FromResult(_context.Posts
            .Where(e => e.Titulo.ToLower().Contains(pesquisa) ||
                        e.Tags.ToLower().Contains(pesquisa) ||
                        e.Descritivo.ToLower().Contains(pesquisa))
            .ToList());
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

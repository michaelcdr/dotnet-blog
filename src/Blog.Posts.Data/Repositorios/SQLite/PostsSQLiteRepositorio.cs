using Blog.Data.Context;
using Blog.Posts.Domain;
using Blog.Posts.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repositorios.SQLite;

public class PostsSQLiteRepositorio : IPostsRepositorio
{
    private AppDbContext _context;

    public PostsSQLiteRepositorio(AppDbContext contexto)
    {
        _context = contexto;
    }

    public List<Post> ObterRecentes()
    {
        return _context.Posts.OrderByDescending(e => e.CadastradoEm).ToList();
    }

    public List<string> ObterTodasTags()
    {
        var tags = new List<string>();
        List<string> tagsDosPosts = _context.Posts.Where(e => !string.IsNullOrEmpty(e.Tags)).Select(e => e.Tags).ToList();
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
        return _context.Posts.Where(e => e.Tags.Contains(tag)).ToList();
    }

    public List<Post> ObterPorCategoria(int id)
    {
        return _context.Posts.Where(e => e.CategoriaId == id).ToList();
    }

    public async Task<Post> Obter(int id)
    {
        Post post = await _context.Posts.SingleAsync(e => e.Id == id);

        Categoria categoria = await _context.Categorias.SingleAsync(e => e.Id == post.CategoriaId);

        post.Categoria = categoria;

        return post;
    }

    public List<Post> ObterPorTermoPesquisa(string pesquisa)
    {
        if (string.IsNullOrEmpty(pesquisa)) return [];

        pesquisa = pesquisa.Trim().ToLower();

        return _context.Posts
            .Where(e => e.Titulo.ToLower().Contains(pesquisa) ||
                        e.Tags.ToLower().Contains(pesquisa) ||
                        e.Descritivo.ToLower().Contains(pesquisa)).ToList();
    }
}
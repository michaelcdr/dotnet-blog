using Blog.Posts.Data.Contexts.SQLite;
using Blog.Posts.Domain;
using Blog.Posts.Domain.Repositories;

namespace Blog.Data.Repositorios.SQLite;

public class CategoriasSQLiteRepositorio : ICategoryRepository
{
    private readonly AppDbContext _contexto;

    public CategoriasSQLiteRepositorio(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<Categoria> ObterPorId(int id)
    {
        return _contexto.Categorias.Single(e => e.Id == id);
    }

    public async Task<List<Categoria>> ObterTodas()
    {
        return _contexto.Categorias.OrderBy(e => e.Nome).ToList();
    }

    public void Criar(Categoria categoria)
    {
        _contexto.Categorias.Add(categoria);
    }

    public async Task Salvar()
    {
        _contexto.SaveChanges();
    }

    public async Task<Categoria?> GetByName(string categoria)
    {
        return _contexto.Categorias.SingleOrDefault(e => e.Nome == categoria);
    }
}
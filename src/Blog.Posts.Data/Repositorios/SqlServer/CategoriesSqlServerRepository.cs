using Blog.Posts.Data.Contexts.SqlServer;
using Blog.Posts.Domain;
using Blog.Posts.Domain.Repositories;

namespace Blog.Data.Repositorios.SqlServer;

public class CategoriesSqlServerRepository : ICategoryRepository
{
    private readonly AppDbContext _contexto;

    public CategoriesSqlServerRepository(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public Task<Categoria> ObterPorId(int id)
    {
        return Task.FromResult(_contexto.Categorias.Single(e => e.Id == id));
    }

    public Task<List<Categoria>> ObterTodas()
    {
        return Task.FromResult(_contexto.Categorias.OrderBy(e => e.Nome).ToList());
    }

    public void Criar(Categoria categoria)
    {
        _contexto.Categorias.Add(categoria);
    }

    public Task Salvar()
    {
        _contexto.SaveChanges();
        return Task.CompletedTask;
    }

    public Task<Categoria?> GetByName(string categoria)
    {
        return Task.FromResult(_contexto.Categorias.SingleOrDefault(e => e.Nome == categoria));
    }
}

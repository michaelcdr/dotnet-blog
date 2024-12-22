using Blog.Posts.Data.Context;
using Blog.Posts.Domain;
using Blog.Posts.Domain.Repositories;

namespace Blog.Posts.Data.Repositorios.InMemory;

public class CategoriasRepositorio : ICategoriasRepositorio
{
    private List<Categoria> _dados = new();
   

    public CategoriasRepositorio(IContexto contexto)
    {
        _dados = contexto.Categorias;
    }

    public void Criar(Categoria categoria)
    {
        _dados.Add(categoria);
    }

    public async Task<Categoria> ObterPorId(int id)
    {
        return _dados.Single(e => e.Id == id);
    }

    public async Task<List<Categoria>> ObterTodas()
    {
        return _dados.OrderBy(e => e.Nome).ToList();
    }

    public async Task Salvar()
    {
        
    }
}
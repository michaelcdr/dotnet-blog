using Blog.Data.Context;
using Blog.Posts.Domain;
using Blog.Posts.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repositorios.SQLite;

public class CategoriasSQLiteRepositorio : ICategoriasRepositorio
{
    private readonly AppDbContext _contexto;

    public CategoriasSQLiteRepositorio(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<Categoria> ObterPorId(int id)
    {
        return await _contexto.Categorias.SingleAsync(e => e.Id == id);
    }

    public async Task<List<Categoria>> ObterTodas()
    {
        return await _contexto.Categorias.OrderBy(e => e.Nome).ToListAsync();
    }

    public void Criar(Categoria categoria)
    {
        _contexto.Categorias.Add(categoria);
    }

    public async Task Salvar()
    {
        await _contexto.SaveChangesAsync();
    }
}
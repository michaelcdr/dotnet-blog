namespace Blog.Domain.Repositories;

public interface ICategoriasRepositorio
{
    Task<List<Categoria>> ObterTodas();
    Task<Categoria> ObterPorId(int id);
    Task Salvar();
    void Criar(Categoria categoria);
}
namespace Blog.Posts.Domain.Repositories;

public interface ICategoryRepository
{
    Task<List<Categoria>> ObterTodas();
    Task<Categoria> ObterPorId(int id);
    Task Salvar();
    void Criar(Categoria categoria);
    Task<Categoria?> GetByName(string categoria);
}
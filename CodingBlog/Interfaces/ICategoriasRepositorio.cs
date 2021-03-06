using CodingBlog.Models;

namespace CodingBlog.Interfaces
{
    public interface ICategoriasRepositorio
    {
        List<Categoria> ObterTodas();
        Categoria ObterPorId(int id);
    }
}
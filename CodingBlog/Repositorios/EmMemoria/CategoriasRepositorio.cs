using CodingBlog.Interfaces;
using CodingBlog.Models;

namespace CodingBlog.Repositorios.EmMemoria
{
    public class CategoriasRepositorio : ICategoriasRepositorio
    {
        private List<Categoria> dados = new()
        {
            new Categoria(1,"C#"),
            new Categoria(2, "AWS")
        };

        public CategoriasRepositorio()
        {
            
        }

        public List<Categoria> ObterTodas()
        {
            return dados.OrderBy(e => e.Nome).ToList();
        }
    }
}
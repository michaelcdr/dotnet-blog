using CodingBlog.Interfaces;
using CodingBlog.Models;

namespace CodingBlog.Repositorios.EmMemoria
{
    public class CategoriasRepositorio : ICategoriasRepositorio
    {
        private List<Categoria> dados = new()
        {
            new Categoria("C#"),
            new Categoria("AWS")
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
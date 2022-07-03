using CodingBlog.Interfaces;
using CodingBlog.Models;
using CodingBlog.Models.Data;

namespace CodingBlog.Repositorios.EmMemoria
{
    public class CategoriasRepositorio : ICategoriasRepositorio
    {
        private List<Categoria> _dados = new();
       

        public CategoriasRepositorio(IContexto contexto)
        {
            _dados = contexto.Categorias;
        }

        public Categoria ObterPorId(int id)
        {
            return _dados.Single(e => e.Id == id);
        }

        public List<Categoria> ObterTodas()
        {
            return _dados.OrderBy(e => e.Nome).ToList();
        }
    }
}
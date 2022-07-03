using CodingBlog.Interfaces;
using CodingBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.ViewComponents.Carrossel
{
    public class CategoriasViewComponent : ViewComponent
    {
        private readonly ICategoriasRepositorio _categoriasRepositorios; 

        public CategoriasViewComponent(ICategoriasRepositorio categoriasRepositorios)
        {
            this._categoriasRepositorios = categoriasRepositorios; 
        }

        public IViewComponentResult Invoke() 
        {
            List<Categoria> categorias = _categoriasRepositorios.ObterTodas();
            return View(categorias);
        }
    }
} 
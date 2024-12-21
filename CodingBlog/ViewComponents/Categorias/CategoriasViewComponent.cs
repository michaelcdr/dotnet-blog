using CodingBlog.HttpClients;
using CodingBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.ViewComponents.Carrossel
{
    public class CategoriasViewComponent : ViewComponent
    {
        private readonly IBlogApiHttpClient _client; 

        public CategoriasViewComponent(IBlogApiHttpClient categoriasRepositorios)
        {
            _client = categoriasRepositorios; 
        }

        public async Task<IViewComponentResult> InvokeAsync() 
        {
            List<CategoriaViewModel> categorias = await _client.ObterCategorias();
            return View(categorias);
        }
    }
} 
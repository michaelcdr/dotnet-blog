using CodingBlog.Services;
using CodingBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.ViewComponents.Carrossel
{
    public class CategoriasViewComponent : ViewComponent
    {
        private readonly IBlogApiService _client; 

        public CategoriasViewComponent(IBlogApiService categoriasRepositorios)
        {
            _client = categoriasRepositorios; 
        }

        public async Task<IViewComponentResult> InvokeAsync() 
        {
            PagedResult<CategoriaViewModel> categorias = await _client.ObterCategorias(pageSize: 50);
            return View(categorias.Items);
        }
    }
} 

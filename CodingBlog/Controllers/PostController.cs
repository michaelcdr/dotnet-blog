using CodingBlog.Interfaces;
using CodingBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.Controllers
{
    public class PostController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoriasRepositorio _categoriasRepositorio;
        private readonly IPostsRepositorio _postsRepositorios; 

        public PostController(
                ILogger<HomeController> logger,
                ICategoriasRepositorio categoriasRepositorio,
                IPostsRepositorio postsRepositorios
            )
        {
            this._logger = logger;
            this._categoriasRepositorio = categoriasRepositorio;
            this._postsRepositorios = postsRepositorios;
        }

        [Route("Post/PorTag/{tag}")]
        public IActionResult PorTag(string tag)
        {  
            var model = new PostsPorTagViewModel(
                _postsRepositorios.ObterPorTags(tag),
                _postsRepositorios.ObterTodasTags(),
                _categoriasRepositorio.ObterTodas()
            );
            return View(model);
        } 

        public IActionResult PorCategoria(int id)
        {  
            var model = new PostsPorCategoriaViewModel(
                _postsRepositorios.ObterPorCategoria(id),
                _postsRepositorios.ObterTodasTags(),
                _postsRepositorios.ObterRecentes(),
                _categoriasRepositorio.ObterTodas()
            );
            return View(model);
        } 

        public IActionResult Detalhes(int id)
        {
            var model = new PostDetalhesViewModel(
                _postsRepositorios.Obter(id),
                _postsRepositorios.ObterTodasTags(),
                _categoriasRepositorio.ObterTodas()
            );
            return View(model);
        }
    }
}
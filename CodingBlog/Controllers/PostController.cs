using CodingBlog.Interfaces;
using CodingBlog.Models;
using CodingBlog.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

        public IActionResult PorTag(string tag)
        {  
            var model = new PostsPorTagViewModel(
                _postsRepositorios.ObterPorTags(tag),
                _postsRepositorios.ObterTodasTags(),
                _postsRepositorios.ObterRecentes(),
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
            return View();
        } 

        public IActionResult Detalhes(int id)
        {
            var model = new PostDetalhesViewModel(
                _postsRepositorios.Obter(id),
                _postsRepositorios.ObterTodasTags(),
                _postsRepositorios.ObterRecentes(),
                _categoriasRepositorio.ObterTodas()
            );
            return View(model);
        }
    }
}
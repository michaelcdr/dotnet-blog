using CodingBlog.Interfaces;
using CodingBlog.Models;
using CodingBlog.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CodingBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoriasRepositorio _categoriasRepositorio;
        private readonly IPostsRepositorio _postsRepositorios;
        private readonly AppDbContext _context;

        public HomeController(
                ILogger<HomeController> logger,
                ICategoriasRepositorio categoriasRepositorio,
                IPostsRepositorio postsRepositorios
            )
        {
            this._logger = logger;
            this._categoriasRepositorio = categoriasRepositorio;
            this._postsRepositorios = postsRepositorios;
        }

        public IActionResult Index()
        { 
            ViewBag.Categorias = _categoriasRepositorio.ObterTodas();
            ViewBag.Tags = _postsRepositorios.ObterTodasTags();
            ViewBag.PostsRecentes = _postsRepositorios.ObterRecentes();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
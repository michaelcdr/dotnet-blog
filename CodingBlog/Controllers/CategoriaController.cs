using CodingBlog.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ICategoriasRepositorio _categoriasRepositorio;
        private readonly IPostsRepositorio _postsRepositorio;

        public CategoriaController(
            ICategoriasRepositorio categoriasRepositorio,
            IPostsRepositorio postsRepositorio)
        {
            this._categoriasRepositorio = categoriasRepositorio;
            this._postsRepositorio = postsRepositorio;
        }

        public IActionResult Index()
        { 
            var categorias = _categoriasRepositorio.ObterTodas(); 
            return View(categorias);
        }

        //public IActionResult Cadastrar()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Cadastrar(Categoria categoria)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // _appDbContext.Categorias.Add(categoria);
        //        // await _appDbContext.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(categoria);
        //}
    }
}

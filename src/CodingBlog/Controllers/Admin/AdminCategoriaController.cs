using CodingBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.Controllers.Admin;

[Route("admin")]
public class AdminCategoriaController : Controller
{
    [Route("categoria")]
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Categories", new { area = "Admin" });
    }

    [Route("categoria/detalhes/{id}")]
    public IActionResult Details(int id)
    {
        return RedirectToAction("Edit", "Categories", new { area = "Admin", id });
    }

    [Route("categoria/cadastrar")]
    public IActionResult Cadastrar()
    {
        return RedirectToAction("Create", "Categories", new { area = "Admin" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CategoriaCadastroModel categoria)
    {
        return RedirectToAction("Create", "Categories", new { area = "Admin" });
    }

    public IActionResult Editar(int id)
    {
        return RedirectToAction("Edit", "Categories", new { area = "Admin", id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(int id, IFormCollection collection)
    {
        return RedirectToAction("Edit", "Categories", new { area = "Admin", id });
    }

    public IActionResult Deletar(int id)
    {
        return RedirectToAction("Delete", "Categories", new { area = "Admin", id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Deletar(int id, IFormCollection collection)
    {
        return RedirectToAction("Delete", "Categories", new { area = "Admin", id });
    }
}
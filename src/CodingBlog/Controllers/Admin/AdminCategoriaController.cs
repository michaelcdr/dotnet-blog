using CodingBlog.Services;
using CodingBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.Controllers.Admin;

[Route("admin")]
public class AdminCategoriaController : Controller
{
    private readonly IBlogApiService _client;

    public AdminCategoriaController(IBlogApiService client)
    {
        _client = client;
    }

    [Route("categoria")]
    public async Task<ActionResult> Index()
    {
        return View(await _client.ObterCategorias());
    }

    [Route("categoria/detalhes/{id}")]
    public ActionResult Details(int id)
    {
        return View();
    }

    [Route("categoria/cadastrar")]
    public ActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Cadastrar(CategoriaCadastroModel categoria)
    {
        if (!ModelState.IsValid) return View(categoria);
        
        await _client.CriarCategoria(categoria);
        return RedirectToAction(nameof(Index));
    }

    public async Task<ActionResult> Editar(int id)
    {
        CategoriaEdicaoModel categoria = await _client.ObterCategoriaPorId(id);
        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    public ActionResult Deletar(int id)
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Deletar(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
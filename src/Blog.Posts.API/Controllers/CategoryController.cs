using Blog.Core.Controller;
using Blog.Posts.API.DTO;
using Blog.Posts.API.Requests;
using Blog.Posts.Data.Contexts.SQLite;
using Blog.Posts.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : MainApiController
{
    private readonly AppDbContext _db;

    public CategoryController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Lista todas categorias.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categorias = await _db.Categorias.AsNoTracking()
            .Select(e => new CategoriaDTO { Id = e.Id, Nome = e.Nome })
            .ToListAsync();
        
        return Ok(categorias);
    }

    /// <summary>
    /// Cadastrar uma nova categoria.
    /// </summary>
    /// <param name="categoria">Nome da categoria.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post(CategoryCreate categoria)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        if (await _db.Categorias.AnyAsync(e => e.Nome == categoria.Nome))
        {
            AddError($"Já existe uma categoria chamada {categoria.Nome}.");
            return CustomResponse();
        }

        _db.Categorias.Add(new Categoria(0, categoria.Nome));
        await _db.SaveChangesAsync();

        return CustomResponse();
    }
}
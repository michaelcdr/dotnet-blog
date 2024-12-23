using Blog.Posts.API.DTOs;
using Blog.Posts.Data.Contexts.SQLite;
using Blog.Posts.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase    
{
    private readonly AppDbContext _db;

    public CategoryController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var categorias = await _db.Categorias.AsNoTracking()
            .Select(e => new CategoriaDTO { Id = e.Id, Nome = e.Nome })
            .ToListAsync();
        
        return Ok(categorias);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CategoriaDTO categoria)
    {
        _db.Categorias.Add(new Categoria(0, categoria.Nome));
        await _db.SaveChangesAsync();

        return Ok();
    }
}
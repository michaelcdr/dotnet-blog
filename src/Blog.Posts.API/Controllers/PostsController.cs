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
public class PostsController : MainApiController
{
    private readonly AppDbContext _db;

    public PostsController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Obtem todos posts
    /// </summary>
    /// <returns>Obtem todos posts</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var posts = await _db.Posts.AsNoTracking()
            .Select(e => new PostDTO { Id = e.Id, Titulo = e.Titulo, Descritivo = e.Descritivo, Imagem = e.Imagem, Tags = e.Tags })
            .ToListAsync();

        return Ok(posts);
    }

    /// <summary>
    /// Obtem um post por id.
    /// </summary>
    /// <param name="id">Id do post.</param>
    /// <returns>Dados do post</returns>
    /// <response code="200">Retorna o post</response>
    /// <response code="404">Se não encontrar o post</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PostDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var post = await _db.Posts.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new PostDTO { Id = e.Id, Titulo = e.Titulo, Descritivo = e.Descritivo, Imagem =e.Imagem, Tags = e.Tags  })
            .SingleOrDefaultAsync();

        if (post == null) return NotFound();

        return Ok(post);
    }

    /// <summary>
    /// Cadastrar uma novo post.
    /// </summary>
    /// <param name="request">Dados do post.</param>
    /// <returns>O post criado.</returns>
    /// <response code="201">Retorna o post criado.</response>
    /// <response code="400">Se os dados estiverem inválidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(PostCreate), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(PostCreate request)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        Categoria? cat = await _db.Categorias.SingleOrDefaultAsync(e => e.Nome == request.Categoria);

        if (cat == null)
        {
            AddError($"A categoria {request.Categoria} não foi encontrada.");
            return CustomResponse();
        }

        var post = new Post(
            0,
            request.Titulo,
            request.Descritivo,
            request.Imagem,
            "",
            string.Join(",", request.Tags),
            cat.Id
        );
        _db.Posts.Add(post);
        await _db.SaveChangesAsync();

        var postDTO = new PostDTO(post);

        return CreatedAtAction(nameof(Get), postDTO, request);
    }

    /// <summary>
    /// Atualiza um post.
    /// </summary>
    /// <param name="request">Dados do post.</param>
    /// <returns>O post criado.</returns>
    /// <response code="200">Retorna o post atualizado.</response>
    /// <response code="400">Se os dados estiverem inválidos</response>
    [HttpPut]
    [ProducesResponseType(typeof(PostUpdate), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(PostUpdate request)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        Categoria? cat = await _db.Categorias.SingleOrDefaultAsync(e => e.Nome == request.Categoria);

        if (cat == null)
        {
            AddError($"A categoria {request.Categoria} não foi encontrada.");
            return CustomResponse();
        }

        Post? post = await _db.Posts.SingleOrDefaultAsync(e => e.Id == request.Id);

        if (post == null)
        {
            AddError($"Não foi encontrado um post com id {request.Id}.");
            return CustomResponse();
        }

        post.Atualizar(request.Titulo, request.Descritivo, request.Imagem, request.Tags, cat.Id, "");

        await _db.SaveChangesAsync();

        var postDTO = new PostDTO(post);

        return CreatedAtAction(nameof(Get), postDTO, request);
    }
}

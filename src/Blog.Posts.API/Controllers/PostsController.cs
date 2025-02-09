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
        IQueryable<Post> postsQuery = _db.Posts.AsNoTracking();

        var posts = await postsQuery
                        .Select(e => new PostResultado
                        {
                            Id = e.Id,
                            Titulo = e.Titulo,
                            Descritivo = e.Descritivo,
                            Imagem = e.Imagem,
                            Tags = e.Tags,
                            Categoria = e.Categoria.Nome,
                            CategoriaId = e.CategoriaId
                        })
            .ToListAsync();

        return Ok(posts);
    }

    /// <summary>
    /// Obtem todos posts
    /// </summary>
    /// <returns>Obtem todos posts</returns>
    [HttpGet("pesquisa/{pesquisa?}")]
    public async Task<IActionResult> Pesquisa([FromRoute] string? pesquisa)
    {
        IQueryable<Post> postsQuery = _db.Posts.AsNoTracking();

        if (!string.IsNullOrEmpty(pesquisa))
            postsQuery = postsQuery.Where(e => e.Titulo.Contains(pesquisa) || e.Tags.Contains(pesquisa) || e.Descritivo.Contains(pesquisa));

        var posts = await postsQuery
                        .Select(e => new PostResultado
                        {
                            Id = e.Id,
                            Titulo = e.Titulo,
                            Descritivo = e.Descritivo,
                            Imagem = e.Imagem,
                            Tags = e.Tags,
                            Categoria = e.Categoria.Nome,
                            CategoriaId = e.CategoriaId
                        })
            .ToListAsync();

        return Ok(posts);
    }

    [HttpGet("por-tag/{tag?}")]
    public async Task<IActionResult> ObterPorTag([FromRoute] string? tag)
    {
        IQueryable<Post> postsQuery = _db.Posts.AsNoTracking();

        if (!string.IsNullOrEmpty(tag))
            postsQuery = postsQuery.Where(e => e.Tags.Contains(tag));

        var posts = await postsQuery
                        .Select(e => new PostResultado
                        {
                            Id = e.Id,
                            Titulo = e.Titulo,
                            Descritivo = e.Descritivo,
                            Imagem = e.Imagem,
                            Tags = e.Tags,
                            Categoria = e.Categoria.Nome,
                            CategoriaId = e.CategoriaId
                        })
            .ToListAsync();

        return Ok(posts);
    }

    [HttpGet("por-categoria/{id}")]
    public async Task<IActionResult> ObterPorCategoria([FromRoute] int id)
    {
        IQueryable<Post> postsQuery = _db.Posts.AsNoTracking().Where(e => e.CategoriaId == id);

        var posts = await postsQuery
                        .Select(e => new PostResultado
                        {
                            Id = e.Id,
                            Titulo = e.Titulo,
                            Descritivo = e.Descritivo,
                            Imagem = e.Imagem,
                            Tags = e.Tags,
                            Categoria = e.Categoria.Nome,
                            CategoriaId = e.CategoriaId
                        })
            .ToListAsync();

        return Ok(posts);
    }

    /// <summary>
    /// Obtem os ultimos 5 posts
    /// </summary>
    /// <returns>Obtem todos posts</returns>
    [HttpGet("recentes")]
    public async Task<IActionResult> ObterRecentes()
    {
        var posts = await _db.Posts.AsNoTracking()
            .Select(e => new PostRecenteResultado { Id = e.Id, Titulo = e.Titulo })
            .Take(5).OrderByDescending(e => e.Id)
            .ToListAsync();

        return Ok(posts);
    }

    /// <summary>
    /// Obtem os ultimos 5 posts
    /// </summary>
    /// <returns>Obtem todos posts</returns>
    [HttpGet("tags")]
    public async Task<IActionResult> ObterTags()
    {
        List<string> tags = await _db.Posts.AsNoTracking().Select(e => e.Tags).ToListAsync();

        var tagsFormatadas = new List<string>();

        foreach (var tag in tags)
            if (!string.IsNullOrEmpty(tag))
                tagsFormatadas.AddRange(tag.Split(",").Select(e => e.Trim()).ToList());

        tagsFormatadas = tagsFormatadas.Distinct().OrderBy(e => e).ToList();

        return Ok(tagsFormatadas);
    }

    /// <summary>
    /// Obtem um post por id.
    /// </summary>
    /// <param name="id">Id do post.</param>
    /// <returns>Dados do post</returns>
    /// <response code="200">Retorna o post</response>
    /// <response code="404">Se não encontrar o post</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PostResultado), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPorId(int id)
    {
        var post = await _db.Posts.AsNoTracking()
            .Where(e => e.Id == id)
                    .Select(e => new PostResultado
                    {
                        Id = e.Id,
                        Titulo = e.Titulo,
                        Descritivo = e.Descritivo,
                        Imagem = e.Imagem,
                        Tags = e.Tags,
                        Categoria = e.Categoria.Nome,
                        CategoriaId = e.CategoriaId
                    })
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

        Categoria? cat = await _db.Categorias.AsNoTracking().SingleOrDefaultAsync(e => e.Nome == request.Categoria);

        if (cat == null)
        {
            AddError($"A categoria {request.Categoria} não foi encontrada.");
            return CustomResponse();
        }
        
        string tags = string.Join(",", request.Tags);
        var post = new Post(0, request.Titulo, request.Descritivo, request.Imagem, "", tags, cat.Id);
        _db.Posts.Add(post);
        await _db.SaveChangesAsync();

        var postDTO = new PostResultado(post);

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

        var postDTO = new PostResultado(post);

        return CreatedAtAction(nameof(Get), postDTO, request);
    }
}

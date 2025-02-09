using CodingBlog.HttpClients;
using CodingBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.Controllers;

public class PostController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBlogApiHttpClient _client;

    public PostController(ILogger<HomeController> logger,
                          IBlogApiHttpClient client)
    {
        _logger = logger;
        _client = client; 
    }

    [Route("Post/PorTag/{tag}")]
    public async Task<IActionResult> PorTag(string tag)
    {  
        ViewBag.Tag = tag;
        List<PostViewModel> posts = await _client.ObterPostsPorTags(tag);
        var model = new PostsPorTagViewModel(posts);
        return View(model);
    } 

    public async Task<IActionResult> PorCategoria(int id)
    {
        PostsPorCategoriaViewModel model = await _client.ObterPostsPorCategoria(id);
        return View(model);
    } 

    public async Task<IActionResult> Detalhes(int id)
    {
        PostViewModel model = await _client.ObterDetalhesPost(id);
        return View(model);
    }

    [Route("Post/Pesquisa/{pesquisa}")]
    public async Task<IActionResult> Pesquisa(string pesquisa)
    {
        ViewBag.Pesquisa = pesquisa;
        List<PostViewModel> posts = await _client.ObterPostsPorTermoPesquisa(pesquisa);
        return View(posts);
    }
}
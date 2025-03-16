using CodingBlog.Services;
using CodingBlog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CodingBlog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBlogApiService _client;

    public HomeController(ILogger<HomeController> logger,
                          IBlogApiService client)
    {
        _logger = logger;
        _client = client; 
    }

    public async Task<IActionResult> Index()
    {
        List<PostViewModel> posts = await _client.ObterPostsPorTermoPesquisa();
        ViewBag.Posts = posts;
        return View();
    } 

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
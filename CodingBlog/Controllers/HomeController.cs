using CodingBlog.HttpClients;
using CodingBlog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CodingBlog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBlogApiHttpClient _client;

    public HomeController(ILogger<HomeController> logger,
                          IBlogApiHttpClient client)
    {
        _logger = logger;
        _client = client; 
    }

    public IActionResult Index()
    {  
        ViewBag.PostsRecentes = _client.ObterPostsRecentes();
        return View();
    } 

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
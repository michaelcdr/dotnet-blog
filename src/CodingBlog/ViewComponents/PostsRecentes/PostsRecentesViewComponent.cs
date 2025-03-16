using CodingBlog.Services;
using CodingBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.ViewComponents.Carrossel;

public class PostsRecentesViewComponent : ViewComponent
{
    private readonly IBlogApiService _client;

    public PostsRecentesViewComponent(IBlogApiService client)
    {
        _client = client;
    }

    public async Task<IViewComponentResult> InvokeAsync() 
    {
        List<PostRecenteViewModel> posts = await _client.ObterPostsRecentes();
        return View(posts);
    }
}
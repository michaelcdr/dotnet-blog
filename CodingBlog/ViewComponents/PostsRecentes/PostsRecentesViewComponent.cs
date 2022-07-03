using CodingBlog.Interfaces;
using CodingBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.ViewComponents.Carrossel
{
    public class PostsRecentesViewComponent : ViewComponent
    {
        private readonly IPostsRepositorio _postsRepositorios; 

        public PostsRecentesViewComponent(IPostsRepositorio postsRepositorios)
        {
            this._postsRepositorios = postsRepositorios; 
        }

        public IViewComponentResult Invoke() 
        {
            List<Post> posts = _postsRepositorios.ObterRecentes();
            return View(posts);
        }
    }
} 
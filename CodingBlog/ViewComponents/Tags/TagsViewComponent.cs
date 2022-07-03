using CodingBlog.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.ViewComponents.Tags
{
    public class TagsViewComponent : ViewComponent
    {
        private readonly IPostsRepositorio _postsRepositorios;

        public TagsViewComponent(IPostsRepositorio postsRepositorios)
        {
            this._postsRepositorios = postsRepositorios;
        }

        public IViewComponentResult Invoke()
        {
            List<string> tags = _postsRepositorios.ObterTodasTags();
            return View(tags);
        }
    }
}

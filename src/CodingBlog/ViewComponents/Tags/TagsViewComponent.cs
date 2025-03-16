using CodingBlog.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.ViewComponents.Tags
{
    public class TagsViewComponent : ViewComponent
    {
        private readonly IBlogApiService _client;

        public TagsViewComponent(IBlogApiService client)
        {
            _client = client;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<string> tags = await _client.ObterTodasTags();
            return View(tags);
        }
    }
}

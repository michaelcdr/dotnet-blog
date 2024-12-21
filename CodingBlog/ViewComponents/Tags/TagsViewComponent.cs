using CodingBlog.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.ViewComponents.Tags
{
    public class TagsViewComponent : ViewComponent
    {
        private readonly IBlogApiHttpClient _client;

        public TagsViewComponent(IBlogApiHttpClient client)
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

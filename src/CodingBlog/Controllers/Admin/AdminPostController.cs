using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.Controllers.Admin;

[Route("admin")]
public class AdminPostController : Controller
{
    [Route("post")]
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Posts", new { area = "Admin" });
    }

    [Route("post/detalhes/{id}")]
    public IActionResult Details(int id)
    {
        return RedirectToAction("Edit", "Posts", new { area = "Admin", id });
    }

    [Route("post/create")]
    public IActionResult Create()
    {
        return RedirectToAction("Create", "Posts", new { area = "Admin" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(IFormCollection collection)
    {
        return RedirectToAction("Create", "Posts", new { area = "Admin" });
    }

    public IActionResult Edit(int id)
    {
        return RedirectToAction("Edit", "Posts", new { area = "Admin", id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, IFormCollection collection)
    {
        return RedirectToAction("Edit", "Posts", new { area = "Admin", id });
    }

    public IActionResult Delete(int id)
    {
        return RedirectToAction("Index", "Posts", new { area = "Admin" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id, IFormCollection collection)
    {
        return RedirectToAction("Index", "Posts", new { area = "Admin" });
    }
}
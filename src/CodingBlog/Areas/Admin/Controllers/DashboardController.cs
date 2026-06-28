using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Posts", new { area = "Admin" });
    }
}

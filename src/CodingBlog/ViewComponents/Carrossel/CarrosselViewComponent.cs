using CodingBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.ViewComponents.Carrossel
{
    public class CarrosselViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration; 

        public CarrosselViewComponent(IConfiguration configuration )
        {
            this._configuration = configuration; 
        }

        public IViewComponentResult Invoke()
        {
            return View(new CarrosselViewModel());
        }
    }
}

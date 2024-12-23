using Blog.Posts.API.DTOs;
using Blog.Posts.Data.Contexts.SQLite;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public PostsController(AppDbContext db)
        {
            _db = db;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await _db.Posts.AsNoTracking()
                .Select(e => new PostDTO { Id = e.Id, Titulo = e.Titulo })
                .ToListAsync();

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task Post(PostDTO postDTO)
        {
     
            await _db.SaveChangesAsync();
        }
    }
}

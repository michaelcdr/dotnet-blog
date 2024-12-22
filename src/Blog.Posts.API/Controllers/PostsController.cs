using Blog.Data.Context;
using Blog.Posts.API.DTOs;
using Blog.Posts.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Posts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public PostsController(AppDbContext appDb)
        {
            _db = appDb;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await _db.Posts.AsNoTracking().Select(e => new PostDTO
            {
                Id = e.Id,
                Titulo = e.Titulo

            }).ToListAsync();

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PostsController>
        [HttpPost]
        public void Post(Post post)
        {
            _db.Posts.Add(post);
        }

        // PUT api/<PostsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PostsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

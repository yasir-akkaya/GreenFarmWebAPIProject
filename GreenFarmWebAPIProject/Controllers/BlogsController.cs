using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenFarmWebAPIProject.Models;
using GreenFarmWebAPIProject.Models.Data;

namespace GreenFarmWebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public BlogsController(ApplicationDbContext context)
        {
            _db = context;
        }

        // GET: api/Blogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
          if (_db.Blogs == null)
          {
              return NotFound();
          }
            return await _db.Blogs.ToListAsync();
        }

        // GET: api/Blogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> GetBlog(int id)
        {
          if (_db.Blogs == null)
          {
              return NotFound();
          }
            var blog = await _db.Blogs.FindAsync(id);

            if (blog == null)
            {
                return NotFound();
            }

            return blog;
        }

        // PUT: api/Blogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog(int id, Blog blog)
        {
            if (id != blog.Id)
            {
                return BadRequest();
            }

            _db.Entry(blog).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Blogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Blog>> PostBlog(Blog blog)
        {
          if (_db.Blogs == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Blogs'  is null.");
          }
            _db.Blogs.Add(blog);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetBlog", new { id = blog.Id }, blog);
        }

        // DELETE: api/Blogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            if (_db.Blogs == null)
            {
                return NotFound();
            }
            var blog = await _db.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            _db.Blogs.Remove(blog);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogExists(int id)
        {
            return (_db.Blogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

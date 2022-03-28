using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Context;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly BlogContext _context;

        public CategoriesController(BlogContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var result = await _context.Categories.ToListAsync();
            if (result.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(result);
            }
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null) return NotFound();

            return category;
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, string title)
        {
            Category category = _context.Categories.Where(c => c.Id == id).FirstOrDefault();
            if (CategoryTitleExists(title) && title != category.Title)
            {
                return Conflict(title);
            }
            category.Title = title;
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(string title)
        {
            if (_context.Categories.Any(c => c.Title == title))
                return Conflict(title);
            else
            {
                _context.Categories.Add(new Category() { Title = title });
                await _context.SaveChangesAsync();

                return Ok(title);
            }
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }


        private bool CategoryTitleExists(string title)
        {
            return _context.Categories.Any(c => c.Title == title);
        }


        // GET: api/Categories/5
        [HttpGet("{id}/posts")]
        public async Task<ActionResult<List<Post>>> GetPosts(int id)
        {
            List<Post> posts = await _context.Posts
                .Where(p => p.Category.Id == id && DateTime.Compare(p.PublicationDate, DateTime.Today) <= 0)
                .OrderByDescending(p => p.PublicationDate).ToListAsync();

            if (posts.Count == 0) return NoContent();
         
            return posts;
        }
    }
}

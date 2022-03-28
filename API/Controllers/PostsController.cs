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
    public class PostsController : ControllerBase
    {
        private readonly BlogContext _context;

        public PostsController(BlogContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            IEnumerable<Post> posts = await _context.Posts.Where(p => DateTime.Compare(p.PublicationDate, DateTime.Today) <= 0).OrderByDescending(p => p.PublicationDate).ToListAsync();
            if (posts.Count() == 0)
                return NoContent();
            else
                return Ok(posts);

        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            Post post = await _context.Posts.Include(i => i.Category).FirstOrDefaultAsync(i => i.Id == id);

            if (post == null) return NotFound();

            return Ok(post);
        }

        // PUT: api/Posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, PostDto postDto)
        {
            Post post = await _context.Posts.Include(i => i.Category).FirstOrDefaultAsync(i => i.Id == id);
            if (PostTitleExists(postDto.Title) && postDto.Title != post.Title)
            {
                return Conflict(postDto.Title);
            }
            if(post.Category.Id != postDto.CategoryId)
            {
                Category category = await _context.Categories.FindAsync(postDto.CategoryId);
                if (category != null)
                    post.Category = category;
            }
            post.Title = postDto.Title;
            post.Content = postDto.Content;
            post.PublicationDate = postDto.PubDate;
            post.Title = postDto.Title;
            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        // POST: api/Posts
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(PostDto post)
        {


            if (PostTitleExists(post.Title)) return Conflict(post.Title);

            Category category = await _context.Categories.FindAsync(post.CategoryId);

            if (post.Title == String.Empty || post.Content == String.Empty || category == null)
                return BadRequest();


            _context.Posts.Add(new Post() { 
                Title = post.Title,
                Content = post.Content,
                Category = category,
                PublicationDate = post.PubDate
            });
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        private bool PostTitleExists(string title)
        {
            return _context.Posts.Any(p => p.Title == title);
        }
    }
}

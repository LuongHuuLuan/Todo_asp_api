using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Contexts;
using TodoApp.DTO.Todos;
using TodoApp.DTO.User;
using TodoApp.Models.Todos;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly TodoDBContext _context;
        //private readonly TodoDBContextSqlServer _context;
        //private readonly TodoDBContextPostgreSQL _context;

        public TagsController(TodoDBContext context)
        {
            _context = context;
        }
        //public TagsController(TodoDBContextSqlServer context)
        //{
        //    _context = context;
        //}
        //public TagsController(TodoDBContextPostgreSQL context)
        //{
        //    _context = context;
        //}

        // GET: api/Tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagResponseDTO>>> GetTags()
        {
            var tags = await _context.Tags.ToListAsync();
            var result = tags.Select(tag => new TagResponseDTO(tag)).ToList();
            return result;
        }

        // GET: api/Tags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TagResponseDTO>> GetTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            return new TagResponseDTO(tag);
        }

        // PUT: api/Tags/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, TagRequestDTO tagDTO)
        {
            var tag = tagDTO.convertToTag();
            tag.Id = id;

            _context.Entry(tag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();


                return Ok(new TagResponseDTO(tag));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Tags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TagResponseDTO>> PostTag(TagRequestDTO tagDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(TagTileExists(tagDTO.Title)) {
                        return BadRequest("Tag title is exist!");
                    }
                    var tag = tagDTO.convertToTag();
                    _context.Tags.Add(tag);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetTag", new { id = tag.Id }, new TagResponseDTO(tag));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }

        private bool TagTileExists(string tagTitle)
        {
            return _context.Tags.Any(e => e.Title == tagTitle);
        }
    }
}

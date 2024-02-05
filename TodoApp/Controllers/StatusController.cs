using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Contexts;
using TodoApp.DTOs.TodoDTOs.StatusDTO;
using TodoApp.Models.Todos;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly TodoDBContext _context;

        public StatusController(TodoDBContext context)
        {
            _context = context;
        }

        // GET: api/Status
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusResponseDTO>>> GetStatus()
        {
            var statuses = await _context.Status.ToListAsync();
            var response = statuses.Select(status => new StatusResponseDTO(status)).ToList();
            return response;
        }

        // GET: api/Status/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StatusResponseDTO>> GetStatus(int id)
        {
            var status = await _context.Status.FindAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            return new StatusResponseDTO((status));
        }

        // PUT: api/Status/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<StatusResponseDTO>> PutStatus(int id, StatusRequestDTO statusDTO)
        {
            var status = statusDTO.convertToStatus();
            status.Id = id;

            _context.Entry(status).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new StatusResponseDTO(status));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Status
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StatusResponseDTO>> PostStatus(StatusRequestDTO statusDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_context.Status.Any(e => e.Title == statusDTO.Title))
                    {
                        return BadRequest("status title is exist!");
                    }
                    else
                    {
                        var status = statusDTO.convertToStatus();
                        _context.Status.Add(status);
                        await _context.SaveChangesAsync();

                        return CreatedAtAction("GetStatus", new { id = status.Id }, new StatusResponseDTO(status));
                    }
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

        // DELETE: api/Status/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var status = await _context.Status.FindAsync(id);
            if (status == null)
            {
                return NotFound();
            }

            _context.Status.Remove(status);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatusExists(int id)
        {
            return _context.Status.Any(e => e.Id == id);
        }
    }
}

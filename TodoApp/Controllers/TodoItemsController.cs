using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Contexts;
using TodoApp.DTOs.TodoDTOs.TodoItemDTO;
using TodoApp.Models.Todos;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoDBContext _context;

        public TodoItemsController(TodoDBContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemResponseDTO>>> GetTodoItems()
        {
            var todoItems = await _context.TodoItems.Include(e => e.Tags).Include(e => e.Status).ToListAsync();
            var result = todoItems.Select(todoItem => new TodoItemResponseDTO(todoItem)).ToList();
            return result;
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemResponseDTO>> GetTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return new TodoItemResponseDTO(todoItem);
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItemResponseDTO>> PutTodoItem(int id, TodoItemRequestDTO todoItemDTO)
        {
            var todoItem = await todoItemDTO.convertToTodoItem(_context);
            todoItem.Id = id;

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new TodoItemResponseDTO(todoItem));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItemResponseDTO>> PostTodoItem(TodoItemRequestDTO todoItemDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var todoItem = await todoItemDTO.convertToTodoItem(_context);
                    _context.TodoItems.Add(todoItem);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, new TodoItemResponseDTO(todoItem));
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

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(int id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}

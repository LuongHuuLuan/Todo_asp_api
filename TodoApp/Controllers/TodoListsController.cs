using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Contexts;
using TodoApp.DTOs.TodoDTOs.TodoListDTO;
using TodoApp.Models.Todos;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly TodoDBContext _context;

        public TodoListsController(TodoDBContext context)
        {
            _context = context;
        }

        // GET: api/TodoLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoListResponseDTO>>> GetTodoList()
        {
            var todolists = await _context.TodoList.Include(e => e.Account)
                                        .Include(e => e.TodoItems)
                                            .ThenInclude(e => e.Tags)
                                        .Include(e => e.TodoItems)
                                            .ThenInclude(e => e.Status)
                                        .ToListAsync();
            var result = todolists.Select(todolist => new TodoListResponseDTO(todolist)).ToList();
            return result;
        }

        // GET: api/TodoLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<TodoListResponseDTO>>> GetTodoList(int id)
        {
            var todoLists = await _context.TodoList.Include(e => e.TodoItems)
                                            .Include(e => e.TodoItems)
                                                .ThenInclude(e => e.Tags)
                                            .Include(e => e.TodoItems)
                                                .ThenInclude(e => e.Status)
                                            .Where(e => e.Account.Id == id).ToListAsync();

            if (todoLists == null)
            {
                return NotFound();
            }

            return todoLists.Select(todoList => new TodoListResponseDTO(todoList)).ToList();
        }

        // PUT: api/TodoLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoList(int id, TodoListRequestDTO todoListDTO)
        {
            var todoList = await todoListDTO.convertToTodoList(_context);
            todoList.Id = id;

            _context.Entry(todoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(todoList);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // POST: api/TodoLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoListResponseDTO>> PostTodoList(TodoListRequestDTO todoListDTO)
        {
            try
            {
                var todoList = await todoListDTO.convertToTodoList(_context);
                _context.TodoList.Add(todoList);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTodoList", new { id = todoList.Id }, new TodoListResponseDTO(todoList));
            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }

        // DELETE: api/TodoLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoList(int id)
        {
            var todoList = await _context.TodoList.FindAsync(id);
            if (todoList == null)
            {
                return NotFound();
            }

            _context.TodoList.Remove(todoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoListExists(int id)
        {
            return _context.TodoList.Any(e => e.Id == id);
        }
    }
}

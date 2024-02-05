using Microsoft.EntityFrameworkCore;
using TodoApp.Contexts;
using TodoApp.Models.Todos;

namespace TodoApp.DTOs.TodoDTOs.TodoListDTO
{
    public class TodoListRequestDTO
    {
        public int accountId { get; set; }
        public string Title { get; set; }

        public async Task<TodoList> convertToTodoList(TodoDBContext context)
        {
            var todoList = new TodoList();
            todoList.Title = this.Title;
            todoList.Account = await context.Accounts.FindAsync(this.accountId);

            return todoList;
        }
    }
}

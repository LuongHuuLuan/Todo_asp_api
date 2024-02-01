using TodoApp.Models.Todos;
using TodoApp.Models.User;

namespace TodoApp.DTO.Todos
{
    public class TodoListResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<TodoItemResponseDTO> TodoItems { get; set; }
        public TodoListResponseDTO(TodoList todoList)
        {
            this.Id = todoList.Id;
            this.Title = todoList.Title;
            this.TodoItems = todoList.TodoItems.Select(todoItem => new TodoItemResponseDTO(todoItem)).ToList();
        }
    }
}

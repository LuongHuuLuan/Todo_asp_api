using TodoApp.DTOs.TodoDTOs.TodoItemDTO;
using TodoApp.Models.Todos;
using TodoApp.Models.User;

namespace TodoApp.DTOs.TodoDTOs.TodoListDTO
{
    public class TodoListResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<TodoItemResponseDTO> TodoItems { get; set; }
        public TodoListResponseDTO(TodoList todoList)
        {
            Id = todoList.Id;
            Title = todoList.Title;
            TodoItems = todoList.TodoItems.Select(todoItem => new TodoItemResponseDTO(todoItem)).ToList();
        }
    }
}

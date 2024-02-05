using System.ComponentModel.DataAnnotations;
using TodoApp.Contexts;
using TodoApp.Models.Todos;

namespace TodoApp.DTOs.TodoDTOs.TodoItemDTO
{
    public class TodoItemRequestDTO
    {
        [Required]
        public string Title { get; set; }

        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string? Description { get; set; }

        public int? statusId { get; set; }

        public ICollection<int>? tagIds { get; set; }

        public int? TodoListId { get; set; }

        public async Task<TodoItem> convertToTodoItem(TodoDBContext context)
        {
            var todoItem = new TodoItem();
            todoItem.Title = this.Title;
            todoItem.TimeStart = this.TimeStart;
            todoItem.TimeEnd = this.TimeEnd;
            todoItem.Description = this.Description;
            todoItem.StatusId = this.statusId;
            var status = await context.Status.FindAsync(todoItem.StatusId);
            todoItem.Status = status;
            foreach (var tagId in tagIds)
            {
                var tag = await context.Tags.FindAsync(tagId);
                if (tag != null)
                {
                    todoItem.Tags.Add(tag);
                }
            }
            todoItem.TodoListId = this.TodoListId;
            var todoList = await context.TodoList.FindAsync(todoItem.TodoListId);
            todoItem.TodoList = todoList;
            return todoItem;
        }
    }
}

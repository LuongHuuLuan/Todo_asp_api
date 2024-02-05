using TodoApp.DTOs.TodoDTOs.StatusDTO;
using TodoApp.DTOs.TodoDTOs.TagDTO;
using TodoApp.Models.Todos;

namespace TodoApp.DTOs.TodoDTOs.TodoItemDTO
{
    public class TodoItemResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string? Description { get; set; }

        public StatusResponseDTO Status { get; set; }

        public ICollection<TagResponseDTO> Tags { get; set; }

        public TodoItemResponseDTO(TodoItem todoItem)
        {
            Id = todoItem.Id;
            Title = todoItem.Title;
            TimeStart = todoItem.TimeStart;
            TimeEnd = todoItem.TimeEnd;
            Description = todoItem.Description;
            Tags = todoItem.Tags.Select(tag => new TagResponseDTO(tag)).ToList();
            Status = new StatusResponseDTO(todoItem.Status);
        }
    }
}

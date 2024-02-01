using TodoApp.Models.Todos;

namespace TodoApp.DTO.Todos
{
    public class TodoItemResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string? Description { get; set; }

        public StatusResponseDTO Status { get; set; }

        public ICollection<TagResponseDTO> Tags { get; set; }

        public TodoItemResponseDTO(TodoItem todoItem)
        {
            this.Id = todoItem.Id;
            this.Title = todoItem.Title;
            this.TimeStart = todoItem.TimeStart;
            this.TimeEnd = todoItem.TimeEnd;
            this.Description = todoItem.Description;
            this.Tags = todoItem.Tags.Select(tag => new TagResponseDTO(tag)).ToList();
            this.Status = new StatusResponseDTO(todoItem.Status);
        }
    }
}

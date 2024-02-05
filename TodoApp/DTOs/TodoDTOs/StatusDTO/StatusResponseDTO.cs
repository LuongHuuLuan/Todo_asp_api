using TodoApp.Models.Todos;

namespace TodoApp.DTOs.TodoDTOs.StatusDTO
{
    public class StatusResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public StatusResponseDTO(Status status)
        {
            Id = status.Id;
            Title = status.Title;
            Description = status.Description;
        }
    }
}

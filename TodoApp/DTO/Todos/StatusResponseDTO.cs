using TodoApp.Models.Todos;

namespace TodoApp.DTO.Todos
{
    public class StatusResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public StatusResponseDTO(Status status)
        {
            this.Id = status.Id;
            this.Title = status.Title;
            this.Description = status.Description;
        }
    }
}

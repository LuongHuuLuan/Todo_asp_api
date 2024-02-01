using TodoApp.Models.Todos;

namespace TodoApp.DTO.Todos
{
    
    public class TagResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public TagResponseDTO(Tag tag)
        {
            this.Id = tag.Id;
            this.Title = tag.Title;
        }
    }
}

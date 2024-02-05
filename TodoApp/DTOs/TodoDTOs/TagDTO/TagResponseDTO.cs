using TodoApp.Models.Todos;

namespace TodoApp.DTOs.TodoDTOs.TagDTO
{

    public class TagResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public TagResponseDTO(Tag tag)
        {
            Id = tag.Id;
            Title = tag.Title;
        }
    }
}

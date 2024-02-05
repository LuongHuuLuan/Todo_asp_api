using System.ComponentModel.DataAnnotations;
using TodoApp.Models.Todos;

namespace TodoApp.DTOs.TodoDTOs.TagDTO
{
    public class TagRequestDTO
    {
        [MaxLength(10)]
        public string Title { get; set; }

        public Tag convertToTag()
        {
            var tag = new Tag();
            tag.Title = Title;
            return tag;
        }
    }
}

using System.ComponentModel.DataAnnotations;
using TodoApp.Models.Todos;

namespace TodoApp.DTOs.TodoDTOs.StatusDTO
{
    public class StatusRequestDTO
    {
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }

        public Status convertToStatus()
        {
            var status = new Status();
            status.Title = this.Title;
            status.Description = this.Description;
            return status;
        }
    }
}

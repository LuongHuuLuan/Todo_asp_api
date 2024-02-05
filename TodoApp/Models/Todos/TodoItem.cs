using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoApp.Models.User;

namespace TodoApp.Models.Todos
{
    public class TodoItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name field is Require.")]
        [StringLength(maximumLength: 255, MinimumLength = 2)]
        public string Title { get; set; }

        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string? Description { get; set; }

        public int? TodoListId { get; set; } // One to one: Optional foreign key property
        public TodoList? TodoList { get; set; } // One to one: Optional reference navigation to principal(TodoList)

        public int? StatusId { get; set; } // One to one: Optional foreign key property
        public Status? Status { get; set; } // One to one: Optional reference navigation to principal(TodoItem)

        // many to many relationship
        public ICollection<Tag> Tags { get; } = new List<Tag>();

    }
}

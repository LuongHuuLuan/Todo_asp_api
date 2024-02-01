using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Models.Todos
{
    [Index(nameof(Tag.Title), IsUnique = true)]
    public class Tag
    {
        public int Id { get; set; }

        public string Title { get; set; } // "Priority, Urgent Tasks", "Work, Personal, Meeting", "Fitness, Study, Exercise"

        // many to many relationship
        public ICollection<TodoItem> TodoItems { get; } = new List<TodoItem>();
    }
}
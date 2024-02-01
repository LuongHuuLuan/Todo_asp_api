using System.Collections;
using TodoApp.Models.User;

namespace TodoApp.Models.Todos
{
    public class TodoList
    {
        public int Id { get; set; }
        public string Title { get; set; }

        //One to many
        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;

        // One to many relationship
        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    }
}

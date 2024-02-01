namespace TodoApp.Models.Todos
{
    public class Status
    {
        public int Id { get; set; }
        public string Title { get; set; } // In Progress, Completed, Pending, Cancel

        public string? Description { get; set; }

        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>(); // One to many todoItem

    }
}

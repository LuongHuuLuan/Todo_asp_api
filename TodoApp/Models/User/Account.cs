using System.ComponentModel.DataAnnotations.Schema;
using TodoApp.Models.JWT;
using TodoApp.Models.Todos;

namespace TodoApp.Models.User
{
    public class Account
    {
        [ForeignKey("People")]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        // One to One
        public int PeopleId { get; set; }
        public virtual People People { get; set; }

        // One to many
        public ICollection<TodoList> TodoLists { get; } = new List<TodoList>(); // Collection navigation containing dependents(TodoList)

        // One to many 
        public ICollection<OutstandingToken> outstandingTokens { get; } = new List<OutstandingToken>();

    }
}

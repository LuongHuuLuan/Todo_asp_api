using System.Linq;
using TodoApp.DTO.Todos;
using TodoApp.Models.User;

namespace TodoApp.DTO.User
{
    public class AccountDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public ICollection<TodoListResponseDTO> TodoLists { get; set; }

        public AccountDTO(Account account)
        {
            this.Id = account.Id;
            this.Username = account.Username;
            this.TodoLists = account.TodoLists.Select(todoList => new TodoListResponseDTO(todoList)).ToList();
        }
    }
}

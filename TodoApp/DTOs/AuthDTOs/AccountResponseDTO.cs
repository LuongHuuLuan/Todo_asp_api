using TodoApp.DTO.User;
using TodoApp.DTOs.TodoDTOs.TodoListDTO;
using TodoApp.Models.User;

namespace TodoApp.DTOs.AuthDTOs
{
    public class AccountResponseDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ContactDTO contacts { get; set; }

        public AccountResponseDTO(People people)
        {
            this.Id = people.Id;
            this.Username = people.Account.Username;
            this.FirstName = people.FirstName;
            this.LastName = people.LastName;
            this.contacts = new ContactDTO(people.Contact);
        }
    }
}

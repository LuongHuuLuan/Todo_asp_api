using TodoApp.Models.User;

namespace TodoApp.DTO.User
{
    public class PeopleDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual Account Account { get; set; }
        public PeopleDTO(People people)
        {
            this.Id = people.Id;
            this.FirstName = people.FirstName;
            this.LastName = people.LastName;
            this.Contact = people.Contact;
            this.Account = people.Account;
        }
    }
}

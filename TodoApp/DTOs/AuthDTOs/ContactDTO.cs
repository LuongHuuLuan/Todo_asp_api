using TodoApp.Models.User;

namespace TodoApp.DTO.User
{
    public class ContactDTO
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public ContactDTO(Contact contact)
        {
            this.Id = contact.Id;
            this.Address = contact.Address;
            this.Email = contact.Email;
            this.Phone = contact.Phone;
            this.Website = contact.Website;
        }
    }
}

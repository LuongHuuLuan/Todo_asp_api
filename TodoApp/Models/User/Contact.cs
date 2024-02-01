using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models.User
{
    public class Contact
    {
        [ForeignKey("People")]
        public int Id { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }

        // One to One
        public int PeopleId { get; set; }
        public virtual People People { get; set; }
    }
}

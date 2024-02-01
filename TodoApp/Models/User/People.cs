using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.User
{
    public class People
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "FirstName field is Require.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "FirstName field is Require.")]
        public string LastName { get; set; }
        
        // One to one relationship
        public virtual Contact Contact { get; set; }

        // One to one relationship
        public virtual Account Account { get; set; }

    }
}

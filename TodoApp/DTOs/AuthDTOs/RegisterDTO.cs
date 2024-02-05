using System.ComponentModel.DataAnnotations;
using TodoApp.Models.User;

namespace TodoApp.DTOs.AuthDTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "FirstName field is Require.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName field is Require.")]
        public string LastName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        [Required(ErrorMessage = "Username field is Require.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password field is Require.")]

        public string Password { get; set; }

    }
}

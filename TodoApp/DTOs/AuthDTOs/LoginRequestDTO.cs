using System.ComponentModel.DataAnnotations;

namespace TodoApp.DTOs.AuthDTOs
{
    public class LoginRequestDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using TodoApp.Models.User;

namespace TodoApp.Models.JWT
{
    public class OutstandingToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public Account Account {  get; set; }
        public string Jti { get; set; }
    }

}

using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models.JWT
{
    public class BlacklistedToken
    {
        public int Id { get; set; }
        public DateTime BlacklistedAt { get; set; }
        [ForeignKey(nameof(OutstandingToken))]
        public int TokenId { get; set; }
        public OutstandingToken Token { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using TodoApp.Contexts;

namespace TodoApp.Repositories
{
    public class UserRepository
    {
        private readonly TodoDBContext _context;

        public UserRepository(TodoDBContext context)
        {
            _context = context;
        }
        public async Task<Boolean> Authenticate(string username, string password)
        {
            var user = await Task.FromResult(_context.Person.Include(e => e.Account).Include(e => e.Contact).SingleOrDefault(person => person.Account.Username == username && person.Account.Password == password));
            if(user != null)
            {
                return true;
            }
            return false;
        }
    }
}

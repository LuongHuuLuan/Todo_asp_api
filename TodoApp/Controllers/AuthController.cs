using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Contexts;
using TodoApp.DTO.User;
using TodoApp.DTOs.AuthDTOs;
using TodoApp.DTOs.TodoDTOs.StatusDTO;
using TodoApp.Models.Todos;
using TodoApp.Models.User;
using TodoApp.Utils;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TodoDBContext _context;

        public AuthController(TodoDBContext context)
        {
            _context = context;
        }

        // GET: api/Status/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountResponseDTO>> GetUserInfo(int id)
        {
            var account = await _context.Person.Include(e => e.Contact).Include(e => e.Account).Where(_ => _.Id == id).FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            return new AccountResponseDTO(account);
        }

        [HttpPost(Name = "Register")]
        public async Task<ActionResult<AccountResponseDTO>> PostStatus(RegisterDTO registerDTO)
        {
            try
            {
                if (_context.Accounts.Any(account => account.Username.Equals(registerDTO.Username) && account.People.Contact.Email.Equals(registerDTO.Email)))
                {
                    return BadRequest("Username or email is exist!");
                }
                else
                {
                    var people = new People();
                    people.FirstName = registerDTO.FirstName;
                    people.LastName = registerDTO.LastName;

                    var account = new Account();
                    account.Username = registerDTO.Username;
                    account.Password = Md5.GennerateMD5(registerDTO.Password);

                    var contact = new Contact();
                    contact.Email = registerDTO.Email;
                    contact.Address = registerDTO.Address;
                    contact.Phone = registerDTO.Phone;
                    contact.Website = registerDTO.Website;

                    people.Account = account;
                    people.Contact = contact;

                    _context.Person.Add(people);


                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetUserInfo", new { id = account.Id }, new AccountResponseDTO(people));
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}

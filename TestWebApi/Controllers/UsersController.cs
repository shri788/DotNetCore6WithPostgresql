#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWebApi.IRepository;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository UserRepo;

        public UsersController(IUserRepository UserRepo)
        {
            this.UserRepo = UserRepo;
        }
        /* private readonly UserDbContext _context;

        public UsersController(UserDbContext context)
        {
            _context = context;
        }
        */
        // GET: api/AllUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            //return await _context.Users.ToListAsync();
            var result = await UserRepo.GetAllUser();
            return Ok(result);
        }

        //Post: api/Users/Add User
        [HttpPost]
        public async Task<ActionResult> AddUser(User User)
        {
            var result = await UserRepo.AddUser(User);
            return Ok(result);
        }
        //Put: api/Users/user
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(User user)
        {
            var result = await UserRepo.EditUser(user);
            return Ok(result);
        }

        //Delete: api/Users/user
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(User user)
        {
            var result = await UserRepo.DeleteUser(user);
            return Ok(result);
        }
        // GET: api/Users/5
        /*[HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.userId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.userId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.userId == id);
        } */
    }
}

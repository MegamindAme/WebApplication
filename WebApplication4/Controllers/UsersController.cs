using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.DataDB;
using WebApplication4.Filters;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TestApiContext _context;

        public UsersController(TestApiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Users
        /// </summary>
        /// <remarks>Get Users List</remarks>
        /// <response code="200">Successfull</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="500">Sorry, Error on our side</response>
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.Include(t => t.Tasks).ToListAsync();
        }

        /// <summary>
        /// Get a User
        /// </summary>
        /// <remarks>Get a User by Id</remarks>
        /// <response code="200">Successfull</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="500">Sorry, Error on our side</response>
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Edit a user
        /// </summary>
        /// <remarks>Edit a user</remarks>
        /// <response code="200">Successfull</response>
        /// <response code="204">Successfully Edited</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="500">Sorry, Error on our side</response>
        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.ID)
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

        /// <summary>
        /// Add a User
        /// </summary>
        /// <remarks>Add a User</remarks>
        /// <response code="200">Successfull</response>
        /// <response code="201">Successfully Created</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="500">Sorry, Error on our side</response>
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'TestApiContext.Users'  is null.");
          }
            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.ID }, user);
        }

        /// <summary>
        /// Delete a User
        /// </summary>
        /// <remarks>Delete a User by Id</remarks>
        /// <response code="200">Successfull</response>
        /// <response code="204">Successfully Deleted</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="500">Sorry, Error on our side</response>
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
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
            return (_context.Users?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}

using Institute_Management.Handler;
using Institute_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensures that only authenticated users can access this controller
    public class UserController : ControllerBase
    {
        private readonly InstituteManagement _context;

        // Constructor to inject the database context
        public UserController(InstituteManagement context)
        {
            _context = context;
        }

        // Get all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        // Get a user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // Create a new user
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            // Check if the user object is valid
            if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.PasswordHash))
            {
                return BadRequest("Invalid user data.");
            }

            // Hash the password before saving
            user.PasswordHash = PasswordHashHandler.HashPassword(user.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Return the created user with a 201 Created status
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // Update an existing user
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (id != updatedUser.Id || string.IsNullOrEmpty(updatedUser.Username))
            {
                return BadRequest("Invalid user data.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Update fields
            user.Username = updatedUser.Username;
            if (!string.IsNullOrWhiteSpace(updatedUser.PasswordHash)) // Allow updating password
            {
                user.PasswordHash = PasswordHashHandler.HashPassword(updatedUser.PasswordHash);
            }

            // Mark the user as modified and save changes
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete a user by ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
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
    }
}
using Institute_Management.Services;
using System;
using Microsoft.AspNetCore.Mvc;
using Institute_Management.Models;
using Microsoft.EntityFrameworkCore;



namespace Institute_Management.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly InstituteManagementDb _context;
        private readonly JwtServices _jwtServices;

        public AuthController(InstituteManagementDb context, JwtServices jwtServices)
        {
            _context = context;
            _jwtServices = jwtServices;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            // Hash the password before saving
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            // Add the user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.PasswordHash))
            {
                return BadRequest("Email and Password are required.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == loginRequest.Email);

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            bool passwordValid = BCrypt.Net.BCrypt.Verify(loginRequest.PasswordHash, user.PasswordHash);
            if (!passwordValid)
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = _jwtServices.GenerateToken(user);
            return Ok(new { token });
        }


    }
}

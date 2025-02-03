using Institute_Management.Handler;
using Institute_Management.Models;
using Institute_Management.Models.api;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Institute_Management.Services
{
    public class JwtServices
    {
        private readonly InstituteManagement _context;
        private readonly IConfiguration _configuration;

        public JwtServices(InstituteManagement context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginRespond> Authenticate(Loginmodules request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == request.UserName);
            if (user == null || !PasswordHashHandler.VerifyPassword(request.Password, user.PasswordHash))
            {
                return null;
            }

            var token = GenerateJwtToken(user);
            return new LoginRespond
            {
                UserName = user.Username,
                AccessToken = token,
                ExpiresIn = (int)TimeSpan.FromMinutes(Convert.ToDouble(_configuration["JwtConfig:TokenValidityMins"])).TotalSeconds
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtConfig:Issuer"],
                audience: _configuration["JwtConfig:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtConfig:TokenValidityMins"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
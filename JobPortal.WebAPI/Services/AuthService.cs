using JobPortal.WebAPI.DTOs;
using JobPortal.WebAPI.Domain;
using JobPortal.WebAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobPortal.WebAPI.Services
{
    public class AuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly string _jwtSecret;

        public AuthService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
                         ?? throw new Exception("JWT_SECRET not set");
        }

        public async Task<AuthResult> RegisterAsync(RegisterRequest request)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email))
                throw new Exception("Email already in use");

            PasswordHasher ph = new PasswordHasher();
            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = ph.Hash(request.Password)
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return new AuthResult
            {
                Token = GenerateJwtToken(user),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && !u.IsDeleted);

            PasswordHasher ph = new PasswordHasher();
            if (user == null || !ph.Verify(request.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            return new AuthResult
            {
                Token = GenerateJwtToken(user),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

public class AuthService
{
    private readonly AutoCvDbContext _context;
    private readonly JwtService _jwtService;

    public AuthService(AutoCvDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<AuthResult?> RegisterAsync(RegisterRequest request)
    {
        Console.WriteLine($"Attempting registration for {request.Email}");

        // Check if user already exists
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new InvalidOperationException("User already exists");

        // Hash the password
        var passwordHash = HashPassword(request.Password);

        // Create new user entity
        var user = new User(
            email: request.Email,
            passwordHash: passwordHash,
            firstName: request.FirstName,
            lastName: request.LastName
        );

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Generate JWT
        var token = _jwtService.GenerateToken(user);

        return new AuthResult
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Token = token
        };
    }

    public async Task<AuthResult?> LoginAsync(LoginRequest request)
    {
        Console.WriteLine($"Attempting login for {request.Email}");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null) return null;

        if (!VerifyPassword(request.Password, user.PasswordHash))
            return null;

        var token = _jwtService.GenerateToken(user);

        return new AuthResult
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Token = token
        };
    }

    // ----- PASSWORD HASHING -----
    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }
}
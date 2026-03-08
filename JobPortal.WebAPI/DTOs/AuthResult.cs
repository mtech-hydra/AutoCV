namespace JobPortal.WebAPI.DTOs;

public record AuthResult
{
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;  // JWT token
}

using JobPortal.WebAPI.Domain;
public interface IJwtService
{
    string GenerateToken(User user);
}
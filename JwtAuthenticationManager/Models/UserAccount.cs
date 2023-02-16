namespace JwtAuthenticationManager.Models;

public class UserAccount
{
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public required string Role { get; init; }
}
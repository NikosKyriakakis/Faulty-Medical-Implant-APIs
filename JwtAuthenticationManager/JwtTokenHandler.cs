using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuthenticationManager.Models;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace JwtAuthenticationManager;

public class JwtTokenHandler
{
    public const string JwtSecurityKey = "5e7ce0e542f5b8abbd4b488cc4";
    private const int JwtTokenValidityMinutes = 60;
    private readonly List<UserAccount> _userAccounts;

    public JwtTokenHandler()
    {
        _userAccounts = new List<UserAccount>
        {
            new() {UserName = "admin", Password = "admin1", Role = "Administrator"},
            new() {UserName = "user", Password = "user1", Role = "User"}
        };
    }

    public AuthenticationResponse? GenerateJwtToken(AuthenticationRequest authenticationRequest)
    {
        if (string.IsNullOrWhiteSpace(authenticationRequest.UserName) ||
            string.IsNullOrWhiteSpace(authenticationRequest.Password))
            return null;

        var userAccount = _userAccounts
            .FirstOrDefault(x => x.UserName == authenticationRequest.UserName && x.Password == authenticationRequest.Password);
        if (userAccount is null)
            return null;

        var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JwtTokenValidityMinutes);
        var tokenKey = Encoding.ASCII.GetBytes(JwtSecurityKey);
        var claimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new(JwtRegisteredClaimNames.Name, authenticationRequest.UserName),
            new(ClaimTypes.Role, userAccount.Role)
        });

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(tokenKey), 
            SecurityAlgorithms.HmacSha256Signature);
        
        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = tokenExpiryTimeStamp,
            SigningCredentials = signingCredentials
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);

        return new AuthenticationResponse
        {
            UserName = userAccount.UserName,
            ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
            JwtToken = token
        };
    }
}
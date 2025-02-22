using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogApi.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]!));
    }

    public string GenerateJwtToken()
    {
         var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, "api-access"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
        var expirationInSeconds = double.Parse(_configuration["JWT:Expiration"] ?? "3600");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            Expires = DateTime.Now.AddSeconds(expirationInSeconds),
            NotBefore = DateTime.Now,
            SigningCredentials = credentials
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
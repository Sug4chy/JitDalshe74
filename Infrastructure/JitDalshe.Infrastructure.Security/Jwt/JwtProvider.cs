using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JitDalshe.Application.Abstractions.Security;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JitDalshe.Infrastructure.Security.Jwt;

public class JwtProvider(IOptions<JwtSettings> jwtOptions) : IJwtProvider
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public string GenerateToken(IdOf<AdminUser> userId, string email, UserRole role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role.ToString())
            ]),
            Expires = DateTime.UtcNow.AddHours(12),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.Post.Tests.Api;

public static class JwtTokenBuilder
{
    public static string CreateValidToken()
    {
        return CreateToken(PostApiFactory.Secret, DateTime.UtcNow.AddHours(1));
    }

    public static string CreateExpiredToken()
    {
        return CreateToken(PostApiFactory.Secret, DateTime.UtcNow.AddMinutes(-10), DateTime.UtcNow.AddMinutes(-20));
    }

    public static string CreateTokenWithInvalidSignature()
    {
        return CreateToken("invalid-secret-key-with-more-than-32-bytes", DateTime.UtcNow.AddHours(1));
    }

    private static string CreateToken(string secret, DateTime expires, DateTime? notBefore = null)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "usuario@teste.com"),
            new(JwtRegisteredClaimNames.Sub, "user-id"),
            new(JwtRegisteredClaimNames.Email, "usuario@teste.com"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, "admin")
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = PostApiFactory.Issuer,
            Audience = PostApiFactory.Audience,
            Subject = new ClaimsIdentity(claims),
            NotBefore = notBefore ?? DateTime.UtcNow.AddMinutes(-1),
            Expires = expires,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateToken(descriptor));
    }
}

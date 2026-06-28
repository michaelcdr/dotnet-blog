using Blog.Auth.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.Auth.Jwt;

public class ClientTokenGenerator : IClientTokenGenerator
{
    private readonly JwtAppSettings _jwtConfig;

    public ClientTokenGenerator(IOptions<JwtAppSettings> options)
    {
        _jwtConfig = options.Value;
    }

    public ClientTokenResponse GenerateAccessToken(string clientId)
    {
        var claims = new ClaimsIdentity([
            new Claim(ClaimTypes.Name, clientId),
            new Claim(JwtRegisteredClaimNames.Sub, clientId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("client_id", clientId)
        ]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtConfig.Issuer,
            Audience = _jwtConfig.Audience,
            Subject = claims,
            Expires = DateTime.UtcNow.AddHours(_jwtConfig.ExpiresIn),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new ClientTokenResponse
        {
            AccessToken = tokenHandler.WriteToken(token),
            ExpiresIn = TimeSpan.FromHours(_jwtConfig.ExpiresIn).TotalSeconds
        };
    }
}

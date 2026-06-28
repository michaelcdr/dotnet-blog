using Blog.Auth.Models;

namespace Blog.Auth.Jwt;

public interface IClientTokenGenerator
{
    ClientTokenResponse GenerateAccessToken(string clientId);
}

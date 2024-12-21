namespace Blog.Auth.Jwt;

public interface ITokenGenerator
{
    Task<TokenGeneratedResponse?> Generate(string userName);
}
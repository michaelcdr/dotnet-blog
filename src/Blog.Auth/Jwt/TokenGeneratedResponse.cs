using Blog.Auth.Models;

namespace Blog.Auth.Jwt
{
    public class TokenGeneratedResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public double ExpiresIn { get; set; }
        public UserToken? UserToken { get; set; }
    }
}
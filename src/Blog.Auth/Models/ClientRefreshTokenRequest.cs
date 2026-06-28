namespace Blog.Auth.Models;

public class ClientRefreshTokenRequest
{
    public string ClientId { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

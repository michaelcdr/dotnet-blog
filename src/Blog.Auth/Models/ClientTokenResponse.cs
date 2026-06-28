namespace Blog.Auth.Models;

public class ClientTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public double ExpiresIn { get; set; }
}

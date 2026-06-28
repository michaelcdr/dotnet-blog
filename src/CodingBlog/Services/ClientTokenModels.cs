namespace CodingBlog.Services;

public class ClientTokenRequest
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}

public class ClientRefreshTokenRequest
{
    public string ClientId { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

public class ClientTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public double ExpiresIn { get; set; }
}

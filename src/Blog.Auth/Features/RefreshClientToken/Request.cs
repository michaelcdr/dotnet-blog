namespace Blog.Auth.Features.RefreshClientToken;

public class Request
{
    public string ClientId { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

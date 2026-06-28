namespace Blog.Auth.Configurations;

public class AuthClientsOptions
{
    public List<AuthClientOptions> Clients { get; set; } = [];
}

public class AuthClientOptions
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}

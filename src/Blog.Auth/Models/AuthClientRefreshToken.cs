namespace Blog.Auth.Models;

public class AuthClientRefreshToken
{
    public Guid Id { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string TokenHash { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public string? ReplacedByTokenHash { get; set; }

    public bool IsActive => RevokedAtUtc == null && ExpiresAtUtc > DateTime.UtcNow;

    public void Revoke(string replacedByTokenHash)
    {
        RevokedAtUtc = DateTime.UtcNow;
        ReplacedByTokenHash = replacedByTokenHash;
    }
}

using Blog.Auth.Configurations;
using Blog.Auth.Data;
using Blog.Auth.Jwt;
using Blog.Auth.Models;
using Blog.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Auth.Services;

public class ClientAuthService : IClientAuthService
{
    private const string ErrorMessage = "Cliente invalido.";
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(7);

    private readonly AuthContext _context;
    private readonly IClientTokenGenerator _tokenGenerator;
    private readonly AuthClientsOptions _options;

    public ClientAuthService(
        AuthContext context,
        IClientTokenGenerator tokenGenerator,
        IOptions<AuthClientsOptions> options)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
        _options = options.Value;
    }

    public async Task<AppResponse<ClientTokenResponse>> GenerateToken(ClientTokenRequest request, CancellationToken cancellationToken = default)
    {
        var client = FindClient(request.ClientId);

        if (client == null || !SecretMatches(client.ClientSecret, request.ClientSecret))
            return Error();

        var response = await CreateTokenResponse(request.ClientId, cancellationToken);

        return new AppResponse<ClientTokenResponse>(true, "Token gerado com sucesso.")
        {
            Data = response
        };
    }

    public async Task<AppResponse<ClientTokenResponse>> RefreshToken(ClientRefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        if (FindClient(request.ClientId) == null)
            return Error();

        var tokenHash = HashToken(request.RefreshToken);
        var refreshToken = await _context.ClientRefreshTokens
            .SingleOrDefaultAsync(token => token.ClientId == request.ClientId && token.TokenHash == tokenHash, cancellationToken);

        if (refreshToken == null || !refreshToken.IsActive)
            return Error();

        var response = await CreateTokenResponse(request.ClientId, cancellationToken);
        refreshToken.Revoke(HashToken(response.RefreshToken));

        await _context.SaveChangesAsync(cancellationToken);

        return new AppResponse<ClientTokenResponse>(true, "Token renovado com sucesso.")
        {
            Data = response
        };
    }

    private async Task<ClientTokenResponse> CreateTokenResponse(string clientId, CancellationToken cancellationToken)
    {
        var response = _tokenGenerator.GenerateAccessToken(clientId);
        var refreshToken = GenerateRefreshToken();

        _context.ClientRefreshTokens.Add(new AuthClientRefreshToken
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            TokenHash = HashToken(refreshToken),
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.Add(RefreshTokenLifetime)
        });

        await _context.SaveChangesAsync(cancellationToken);

        response.RefreshToken = refreshToken;
        return response;
    }

    private AuthClientOptions? FindClient(string clientId)
    {
        return _options.Clients.SingleOrDefault(client => client.ClientId == clientId);
    }

    private static bool SecretMatches(string expectedSecret, string providedSecret)
    {
        var expectedBytes = Encoding.UTF8.GetBytes(expectedSecret);
        var providedBytes = Encoding.UTF8.GetBytes(providedSecret);

        return expectedBytes.Length == providedBytes.Length
            && CryptographicOperations.FixedTimeEquals(expectedBytes, providedBytes);
    }

    private static string GenerateRefreshToken()
    {
        return Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(64));
    }

    private static string HashToken(string token)
    {
        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    }

    private static AppResponse<ClientTokenResponse> Error()
    {
        return new AppResponse<ClientTokenResponse>(false, ErrorMessage, [
            new Notification(ErrorMessage, string.Empty)
        ]);
    }
}

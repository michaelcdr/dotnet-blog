using CodingBlog.Configuracoes;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace CodingBlog.Services;

public class BlogAuthClientTokenProvider : IBlogAuthClientTokenProvider
{
    private readonly SemaphoreSlim _tokenLock = new(1, 1);

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppSettings _settings;
    private ClientTokenResponse? _currentToken;
    private DateTimeOffset _expiresAtUtc;

    public BlogAuthClientTokenProvider(IHttpClientFactory httpClientFactory, IOptions<AppSettings> options)
    {
        _httpClientFactory = httpClientFactory;
        _settings = options.Value;
    }

    public async Task<string> GetAccessToken(CancellationToken cancellationToken = default)
    {
        if (TokenIsValid())
            return _currentToken!.AccessToken;

        await _tokenLock.WaitAsync(cancellationToken);
        try
        {
            if (TokenIsValid())
                return _currentToken!.AccessToken;

            if (!string.IsNullOrWhiteSpace(_currentToken?.RefreshToken))
                return await RefreshAccessTokenCore(cancellationToken);

            return await GenerateAccessToken(cancellationToken);
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    public async Task<string> RefreshAccessToken(CancellationToken cancellationToken = default)
    {
        await _tokenLock.WaitAsync(cancellationToken);
        try
        {
            if (!string.IsNullOrWhiteSpace(_currentToken?.RefreshToken))
                return await RefreshAccessTokenCore(cancellationToken);

            return await GenerateAccessToken(cancellationToken);
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    private async Task<string> GenerateAccessToken(CancellationToken cancellationToken)
    {
        var request = new ClientTokenRequest
        {
            ClientId = _settings.BlogAuthClient.ClientId,
            ClientSecret = _settings.BlogAuthClient.ClientSecret
        };

        var response = await CreateClient().PostAsJsonAsync("api/clients/token", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var token = await response.Content.ReadFromJsonAsync<ClientTokenResponse>(cancellationToken);
        SetToken(token);

        return _currentToken!.AccessToken;
    }

    private async Task<string> RefreshAccessTokenCore(CancellationToken cancellationToken)
    {
        var request = new ClientRefreshTokenRequest
        {
            ClientId = _settings.BlogAuthClient.ClientId,
            RefreshToken = _currentToken!.RefreshToken
        };

        var response = await CreateClient().PostAsJsonAsync("api/clients/refresh-token", request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _currentToken = null;
            return await GenerateAccessToken(cancellationToken);
        }

        var token = await response.Content.ReadFromJsonAsync<ClientTokenResponse>(cancellationToken);
        SetToken(token);

        return _currentToken!.AccessToken;
    }

    private void SetToken(ClientTokenResponse? token)
    {
        if (token == null || string.IsNullOrWhiteSpace(token.AccessToken))
            throw new InvalidOperationException("Nao foi possivel obter token de acesso para a API.");

        _currentToken = token;
        _expiresAtUtc = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn);
    }

    /// <summary>
    /// Verify if token has expired.
    /// </summary>
    /// <returns></returns>
    private bool TokenIsValid()
    {
        return !string.IsNullOrWhiteSpace(_currentToken?.AccessToken)
            && _expiresAtUtc > DateTimeOffset.UtcNow.AddMinutes(1);
    }

    private HttpClient CreateClient()
    {
        return _httpClientFactory.CreateClient("BlogAuthClient");
    }
}

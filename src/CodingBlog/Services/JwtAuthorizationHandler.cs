using System.Net;

namespace CodingBlog.Services;

public class JwtAuthorizationHandler : DelegatingHandler
{
    private readonly IBlogAuthClientTokenProvider _tokenProvider;

    public JwtAuthorizationHandler(IBlogAuthClientTokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        await AddAuthorizationHeader(request, cancellationToken);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        response.Dispose();

        var retryRequest = await CloneRequest(request, cancellationToken);
        await AddAuthorizationHeader(retryRequest, cancellationToken, forceRefresh: true);

        return await base.SendAsync(retryRequest, cancellationToken);
    }

    private async Task AddAuthorizationHeader(
        HttpRequestMessage request,
        CancellationToken cancellationToken,
        bool forceRefresh = false)
    {
        var token = forceRefresh
            ? await _tokenProvider.RefreshAccessToken(cancellationToken)
            : await _tokenProvider.GetAccessToken(cancellationToken);

        request.Headers.Authorization = new("Bearer", token);
    }

    private static async Task<HttpRequestMessage> CloneRequest(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version,
            VersionPolicy = request.VersionPolicy
        };

        foreach (var header in request.Headers)
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

        if (request.Content != null)
        {
            var content = await request.Content.ReadAsByteArrayAsync(cancellationToken);
            clone.Content = new ByteArrayContent(content);

            foreach (var header in request.Content.Headers)
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return clone;
    }
}

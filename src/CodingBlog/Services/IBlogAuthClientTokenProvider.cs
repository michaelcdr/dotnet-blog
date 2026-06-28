namespace CodingBlog.Services;

public interface IBlogAuthClientTokenProvider
{
    Task<string> GetAccessToken(CancellationToken cancellationToken = default);
    Task<string> RefreshAccessToken(CancellationToken cancellationToken = default);
}

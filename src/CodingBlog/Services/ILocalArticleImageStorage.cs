using Microsoft.AspNetCore.Http;

namespace CodingBlog.Services;

public interface ILocalArticleImageStorage
{
    Task<string?> SaveAsync(IFormFile? file, CancellationToken cancellationToken = default);
}

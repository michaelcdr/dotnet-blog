using Microsoft.AspNetCore.Http;

namespace CodingBlog.Services;

public class LocalArticleImageStorage : ILocalArticleImageStorage
{
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".gif"
    };

    private readonly IWebHostEnvironment _environment;

    public LocalArticleImageStorage(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string?> SaveAsync(IFormFile? file, CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
            return null;

        var extension = Path.GetExtension(file.FileName);

        if (!AllowedExtensions.Contains(extension))
            throw new InvalidOperationException("Envie uma imagem valida nos formatos JPG, PNG, WEBP ou GIF.");

        var fileName = $"artigo-{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var directory = Path.Combine(_environment.WebRootPath, "img", "artigos");

        Directory.CreateDirectory(directory);

        var path = Path.Combine(directory, fileName);

        await using var stream = File.Create(path);
        await file.CopyToAsync(stream, cancellationToken);

        return fileName;
    }
}

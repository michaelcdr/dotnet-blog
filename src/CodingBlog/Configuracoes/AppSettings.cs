namespace CodingBlog.Configuracoes;

public class AppSettings
{
    public string UrlPostsApi { get; set; } = string.Empty;
    public string UrlAuthApi { get; set; } = string.Empty;
    public BlogAuthClientSettings BlogAuthClient { get; set; } = new();
    public VisualSettings VisualSettings { get; set; } = new();
}

public class BlogAuthClientSettings
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}

public class VisualSettings
{
    public string Layout { get; set; } = "classic";
}

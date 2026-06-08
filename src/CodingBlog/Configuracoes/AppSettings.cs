namespace CodingBlog.Configuracoes;

public class AppSettings
{
    public string UrlPostsApi { get; set; } = string.Empty;
    public string UrlAuthApi { get; set; } = string.Empty;
    public VisualSettings VisualSettings { get; set; } = new();
}

public class VisualSettings
{
    public string Layout { get; set; } = "classic";
}

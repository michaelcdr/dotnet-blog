using Blog.Posts.Data.Contexts.SqlServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Blog.Post.Tests.Api;

public sealed class PostApiFactory : WebApplicationFactory<Program>
{
    public const string Secret = "post-tests-secret-key-with-more-than-32-bytes";
    public const string Issuer = "blog-auth";
    public const string Audience = "blog-posts-api";

    private readonly string _databaseName = $"CodingBlog_Posts_Tests_{Guid.NewGuid():N}";
    private readonly string _connectionString;

    public PostApiFactory()
    {
        _connectionString = $"Server=(localdb)\\SGPLocalDB;Database={_databaseName};Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;Encrypt=False";
    }

    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("ConnectionStrings:PostsDb", _connectionString);
        builder.UseSetting("JwtAppSettings:Secret", Secret);
        builder.UseSetting("JwtAppSettings:Issuer", Issuer);
        builder.UseSetting("JwtAppSettings:Audience", Audience);
        builder.UseSetting("JwtAppSettings:ExpiresIn", "2");

        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            var json = JsonSerializer.Serialize(new
            {
                ConnectionStrings = new
                {
                    PostsDb = _connectionString
                },
                JwtAppSettings = new
                {
                    Secret,
                    Issuer,
                    Audience,
                    ExpiresIn = 2
                }
            });

            configuration.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        });
    }

    public HttpClient CreateAuthenticatedClient(string token)
    {
        var client = CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });

        client.DefaultRequestHeaders.Authorization = new("Bearer", token);
        return client;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            try
            {
                using var scope = Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureDeleted();
            }
            catch
            {
                // Cleanup must not hide the test failure that triggered disposal.
            }
        }

        base.Dispose(disposing);
    }
}

using Blog.Auth.Data;
using Blog.Auth.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Blog.Auth.Configurations;

public static class IdentityConfig
{
    private const string DefaultSqlServerConnection = "Server=MIKA-DESK\\SQLEXPRESS;Database=CodingBlog_Auth;User Id=michael;Password=giacom;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;Encrypt=False";

    public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var isDevelopment = string.Equals(configuration["ASPNETCORE_ENVIRONMENT"], "Development", StringComparison.OrdinalIgnoreCase);
        var defaultConnection = isDevelopment
            ? DefaultSqlServerConnection
            : configuration.GetConnectionString("DefaultConnection") ?? DefaultSqlServerConnection;

        services.AddDbContext<AuthContext>(opt => opt.UseSqlServer(defaultConnection));

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthContext>()
            .AddErrorDescriber<IdentityPortuguesMessages>()
            .AddDefaultTokenProviders();

        AddJWTConfiguration(services, configuration);
        AddClientConfiguration(services, configuration);

        services.Configure<IdentityOptions>(opt =>
        {
            opt.Password.RequireDigit = false;
            opt.Password.RequiredLength = 5;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireLowercase = true;
            opt.Password.RequiredUniqueChars = 3;
            opt.User.RequireUniqueEmail = true;
            opt.Lockout.AllowedForNewUsers = true;
            opt.Lockout.MaxFailedAccessAttempts = 5;
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        });

        return services;
    }

    public static IApplicationBuilder UseIdentity(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    private static void AddJWTConfiguration(IServiceCollection services,
                                            IConfiguration configuration)
    {
        var appSettingsSection = configuration.GetSection("JwtAppSettings");
        services
            .AddOptions<JwtAppSettings>()
            .Bind(appSettingsSection)
            .Validate(settings => !string.IsNullOrWhiteSpace(settings.Issuer), "JwtAppSettings:Issuer deve ser informado.")
            .Validate(settings => !string.IsNullOrWhiteSpace(settings.Audience), "JwtAppSettings:Audience deve ser informado.")
            .Validate(settings => settings.ExpiresIn > 0, "JwtAppSettings:ExpiresIn deve ser maior que zero.")
            .Validate(settings => Encoding.UTF8.GetByteCount(settings.Secret ?? string.Empty) >= 32, "JwtAppSettings:Secret deve ter pelo menos 32 bytes.")
            .ValidateOnStart();

        var appSettings = appSettingsSection.Get<JwtAppSettings>();
        var key = Encoding.UTF8.GetBytes(appSettings?.Secret ?? string.Empty);

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, bearerOpts => {

            bearerOpts.RequireHttpsMetadata = true;
            bearerOpts.SaveToken = true;
            bearerOpts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidAudience = appSettings?.Audience,
                ValidIssuer = appSettings?.Issuer
            };
        });
    }

    private static void AddClientConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<AuthClientsOptions>()
            .Bind(configuration.GetSection("AuthClients"))
            .Validate(options => options.Clients.Count > 0, "AuthClients:Clients deve possuir ao menos um cliente.")
            .Validate(options => options.Clients.All(client => !string.IsNullOrWhiteSpace(client.ClientId)), "AuthClients:ClientId deve ser informado.")
            .Validate(options => options.Clients.All(client => Encoding.UTF8.GetByteCount(client.ClientSecret ?? string.Empty) >= 32), "AuthClients:ClientSecret deve ter pelo menos 32 bytes.")
            .ValidateOnStart();
    }
}

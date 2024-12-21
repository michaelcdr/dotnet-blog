using Blog.Auth.Jwt;
using Blog.Auth.Services;

namespace Blog.Auth.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddServicesConfig(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        return services;
    }
}

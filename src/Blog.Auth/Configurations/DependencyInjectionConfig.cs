using Blog.Auth.Jwt;
using Blog.Auth.Services;
using FluentValidation;

namespace Blog.Auth.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddServicesConfig(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IClientAuthService, ClientAuthService>();
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IClientTokenGenerator, ClientTokenGenerator>();
        services.AddScoped<Features.Login.Handler>();
        services.AddScoped<IValidator<Features.Login.Request>, Features.Login.Validator>();
        services.AddScoped<Features.GenerateClientToken.Handler>();
        services.AddScoped<IValidator<Features.GenerateClientToken.Request>, Features.GenerateClientToken.Validator>();
        services.AddScoped<Features.RefreshClientToken.Handler>();
        services.AddScoped<IValidator<Features.RefreshClientToken.Request>, Features.RefreshClientToken.Validator>();
        return services;
    }
}

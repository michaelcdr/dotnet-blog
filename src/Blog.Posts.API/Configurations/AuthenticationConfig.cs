using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Blog.Posts.API.Configurations;

public static class AuthenticationConfig
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
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

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);

        services
            .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtAppSettings>>((options, jwtOptions) =>
            {
                var appSettings = jwtOptions.Value;
                var key = Encoding.UTF8.GetBytes(appSettings.Secret);

                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidIssuer = appSettings.Issuer,
                    ValidAudience = appSettings.Audience
                };
            });

        return services;
    }

    public static IMvcBuilder AddAuthenticatedControllers(this IServiceCollection services)
    {
        return services.AddControllers(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.Filters.Add(new AuthorizeFilter(policy));
        });
    }
}

using Blog.Auth.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Blog.Auth.Configurations;

public static class APIConfig
{
    public static IServiceCollection AddAPIConfig(this IServiceCollection services,
                                                  ConfigurationManager configuration,
                                                  IWebHostEnvironment environment)
    {
        configuration
            .SetBasePath(environment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        if (environment.IsDevelopment())
        {
            configuration.AddUserSecrets<Program>();
        }

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        return services;
    }

    public static IApplicationBuilder UseAPIConfig(this IApplicationBuilder app, IWebHostEnvironment hostEnvironment)
    {
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseIdentity();

        app.UseEndpoints(endpo =>
        {
            endpo.MapControllers();
        });

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            if (!roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
            {
                var result = roleManager.CreateAsync(new IdentityRole("admin")).GetAwaiter().GetResult();
            }

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            IdentityUser? user = userManager.FindByNameAsync("michael").GetAwaiter().GetResult();

            if (user == null)
            {
                user = new IdentityUser("michael");
                userManager.CreateAsync(user, "123456").GetAwaiter().GetResult();
            }

            var rolesUser = userManager.GetRolesAsync(user).GetAwaiter().GetResult();

            if (!rolesUser.Any(e => e == "admin"))
                userManager.AddToRoleAsync(user, "admin").GetAwaiter().GetResult() ;

        }
        return app;
    }
}
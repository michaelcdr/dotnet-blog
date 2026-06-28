using Blog.Auth.Data;
using Blog.Auth.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        if (!hostEnvironment.IsDevelopment())
            app.UseHttpsRedirection();

        app.UseRouting();

        app.UseIdentity();

        app.UseEndpoints(endpo =>
        {
            endpo.MapControllers();
        });

        if (!hostEnvironment.IsDevelopment())
            return app;

        using var scope = app.ApplicationServices.CreateScope();

        var authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
        authContext.Database.Migrate();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
        {
            var result = roleManager.CreateAsync(new IdentityRole("admin")).GetAwaiter().GetResult();
            if (!result.Succeeded)
                throw new InvalidOperationException("Nao foi possivel criar a role admin de desenvolvimento.");
        }

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        IdentityUser? user = userManager.FindByNameAsync("michael").GetAwaiter().GetResult();

        if (user == null)
        {
            user = new IdentityUser("michael")
            {
                Email = "michael@codingblog.local",
                EmailConfirmed = true
            };

            var result = userManager.CreateAsync(user, "giacom").GetAwaiter().GetResult();
            if (!result.Succeeded)
                throw new InvalidOperationException("Nao foi possivel criar o usuario admin de desenvolvimento.");
        }
        else
        {
            user.Email = "michael@codingblog.local";
            user.EmailConfirmed = true;

            if (userManager.HasPasswordAsync(user).GetAwaiter().GetResult())
            {
                var removePasswordResult = userManager.RemovePasswordAsync(user).GetAwaiter().GetResult();
                if (!removePasswordResult.Succeeded)
                    throw new InvalidOperationException("Nao foi possivel atualizar a senha do usuario admin de desenvolvimento.");
            }

            var addPasswordResult = userManager.AddPasswordAsync(user, "giacom").GetAwaiter().GetResult();
            if (!addPasswordResult.Succeeded)
                throw new InvalidOperationException("Nao foi possivel definir a senha do usuario admin de desenvolvimento.");

            user.AccessFailedCount = 0;
            user.LockoutEnd = null;
            var updateResult = userManager.UpdateAsync(user).GetAwaiter().GetResult();
            if (!updateResult.Succeeded)
                throw new InvalidOperationException("Nao foi possivel atualizar o usuario admin de desenvolvimento.");
        }

        var rolesUser = userManager.GetRolesAsync(user).GetAwaiter().GetResult();

        if (!rolesUser.Any(e => e == "admin"))
        {
            var result = userManager.AddToRoleAsync(user, "admin").GetAwaiter().GetResult();
            if (!result.Succeeded)
                throw new InvalidOperationException("Nao foi possivel associar o usuario de desenvolvimento a role admin.");
        }

        return app;
    }
}

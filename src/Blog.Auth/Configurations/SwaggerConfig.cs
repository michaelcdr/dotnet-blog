﻿using Microsoft.OpenApi.Models;

namespace Blog.Auth.Configurations;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Marketplace - API de Autenticação",
                Description = "API de Autenticação",
                Contact = new OpenApiContact
                {
                    Name = "Michael Costa dos Reis",
                    Email = "michaelcdr@hotmail.com"
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });
        });
        return services;
    }

    public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app, IWebHostEnvironment hostEnvironment)
    {
        if (hostEnvironment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
        return app;
    }
}
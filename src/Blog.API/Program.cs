using Blog.Data.Context;
using Blog.Data.Repositorios.SQLite;
using Blog.Domain;
using Blog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connection = builder.Configuration["ConexaoSqlite:SqliteConnectionString"];
builder.Services.AddSqlite<AppDbContext>(connection);

//builder.Services.AddTransient<IContexto, Contexto>();
//builder.Services.AddTransient<ICategoriasRepositorio,CategoriasRepositorio>();
//builder.Services.AddTransient<IPostsRepositorio,PostsRepositorio>();

builder.Services.AddScoped<ICategoriasRepositorio, CategoriasSQLiteRepositorio>();
builder.Services.AddScoped<IPostsRepositorio, PostsSQLiteRepositorio>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

async Task VerificarDBExiste(IServiceProvider services, ILogger logger)
{
    logger.LogInformation(
        "Garantindo que o banco de dados exista e esteja na string de conexão :" +
        " '{connectionString}'", connection
    );

    using var db = services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    int qtdPendingMigrations = db.Database.GetPendingMigrations().Count();

    if (qtdPendingMigrations > 0)
    {
        logger.LogInformation("Tem migrations pendentes atualizando");
        await db.Database.MigrateAsync();
    }

    if (!await db.Categorias.AnyAsync())
    {
        var categoria = new Categoria(0, "Categoria 1");
        db.Categorias.Add(categoria);
        await db.SaveChangesAsync();


        if (!await db.Posts.AnyAsync())
        {
            db.Posts.Add(new Post(0, "Post 1", "fdasfd", null, "adfasfd", "fasfda", categoria.Id, DateTime.Now));
            db.Posts.Add(new Post(0, "Post 2", "fdasfd", null, "adfasfd", "fasfda", categoria.Id, DateTime.Now));
            await db.SaveChangesAsync();
        }
    }
}
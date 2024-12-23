using Blog.Core.Bus;
using Blog.Data.Repositorios.SQLite;
using Blog.Posts.Data.Contexts.SQLite;
using Blog.Posts.Domain;
using Blog.Posts.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//var connection = builder.Configuration["ConexaoSqlite:SqliteConnectionString"];
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db")
);
builder.Services.AddScoped<IMediatrHandler, MediatrHandler>();
builder.Services.AddScoped<ICategoryRepository, CategoriasSQLiteRepositorio>();
builder.Services.AddScoped<IPostRepository, PostsSQLiteRepositorio>();

// in memory
//builder.Services.AddTransient<IContexto, Contexto>();
//builder.Services.AddTransient<ICategoriasRepositorio,CategoriasRepositorio>();
//builder.Services.AddTransient<IPostsRepositorio,PostsRepositorio>(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

await VerificarDBExiste(app.Services, logger);

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
        " '{connectionString}'",
        "Data Source=app.db"
    );

    using var db = services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    bool temBanco = await db.Database.EnsureCreatedAsync();

    //int qtdPendingMigrations = db.Database.GetPendingMigrations().Count();

    //if (qtdPendingMigrations > 0)
    //{
    //    logger.LogInformation("Tem migrations pendentes atualizando");
    //    await db.Database.MigrateAsync();
    //}

    if (!await db.Categorias.AnyAsync())
    {
        var categoria = new Categoria(0, "Categoria 1", new List<Post>
        {
            new Post(0, "Post 1", "fdasfd", null, "adfasfd", "fasfda", 0, DateTime.Now),
            new Post(0, "Post 2", "fdasfd", null, "adfasfd", "fasfda", 0, DateTime.Now)
        });
        
        db.Categorias.Add(categoria);

        await db.SaveChangesAsync();
    }
}
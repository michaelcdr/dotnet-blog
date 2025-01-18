using Blog.Core.Bus;
using Blog.Data.Repositorios.SQLite;
using Blog.Posts.Data.Contexts.SQLite;
using Blog.Posts.Domain;
using Blog.Posts.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API de Exemplo",
        Description = "Uma API de exemplo usando .NET 8"
    });

    // Configura o Swagger para usar o arquivo XML de comentários
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

});

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

    if (!await db.Categorias.AnyAsync())
    {
        var categoria = new Categoria(0, "Dapper", new List<Post>
        {
            new Post(
                0, 
                "Operações de CRUD usando Dapper",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas et lorem ligula. Nunc fringilla " +
                "sem at mi iaculis rutrum. Duis convallis nulla eget lacus commodo accumsan. Maecenas porttitor, " +
                "nisi nec condimentum finibus, erat lorem scelerisque est, eget rutrum ligula mauris in felis. " +
                "Fusce luctus sollicitudin auctor. Nam ultrices sodales sem eget finibus. Donec sodales, sem at " +
                "iaculis lobortis, orci tellus accumsan arcu, lacinia commodo sem ligula ut tellus. Vestibulum " +
                "dignissim volutpat interdum. Pellentesque malesuada placerat mauris a sagittis. Suspendisse potenti. " +
                "Suspendisse orci urna, tempus a mattis vel, vestibulum id lacus.\r\n\r\nDuis vel varius tellus, at lacinia arcu. " +
                "Nullam venenatis sed ipsum eu semper. Duis nec convallis nunc, viverra faucibus est. Vestibulum pulvinar risus ac " +
                "venenatis pellentesque. Aenean non ligula placerat, convallis erat quis, pharetra eros. Sed condimentum consectetur " +
                "dui non tincidunt. Praesent interdum tellus in dui tristique, sit amet accumsan leo fermentum. Sed laoreet sollicitudin " +
                "dignissim. Aenean at felis lacinia, malesuada ligula et, fringilla metus. Sed tincidunt placerat consectetur. " +
                "Curabitur non nunc neque. Morbi consectetur eget mi eget tristique. Mauris cursus odio ut justo vulputate, " +
                "non eleifend tellus dapibus. Mauris tincidunt orci at fermentum suscipit.", 
                null,
                "Michael Costa dos Reis", 
                "Dapper, .NET, Crud, C#", 
                0, 
                DateTime.Now
            ) 
        });
        
        db.Categorias.Add(categoria);

        await db.SaveChangesAsync();
    }
}
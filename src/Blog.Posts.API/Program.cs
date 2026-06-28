using Blog.Core.Bus;
using Blog.Data.Repositorios.SqlServer;
using Blog.Posts.API.Configurations;
using Blog.Posts.Data.Contexts.SqlServer;
using Blog.Posts.Domain;
using Blog.Posts.Domain.Events;
using Blog.Posts.Domain.Repositories;
using Blog.Posts.Domain.Services.CadastrarPost;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddJwtAuthentication(builder.Configuration)
    .AddAuthenticatedControllers();

const string defaultPostsConnection = "Server=MIKA-DESK\\SQLEXPRESS;Database=CodingBlog_Posts;User Id=michael;Password=giacom;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;Encrypt=False";
var connectionString = builder.Configuration.GetConnectionString("PostsDb") ?? defaultPostsConnection;

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(UpdateQtdPostsCategoryEvent).Assembly));

builder.Services.AddScoped<IMediatrHandler, MediatrHandler>();
builder.Services.AddScoped<ICategoryRepository, CategoriesSqlServerRepository>();
builder.Services.AddScoped<IPostRepository, PostsSqlServerRepository>();
builder.Services.AddScoped<ICadastrarPost, CadastrarPost>();
builder.Services.AddScoped<Blog.Posts.API.Features.Posts.ListPosts.Handler>();
builder.Services.AddScoped<Blog.Posts.API.Features.Posts.SearchPosts.Handler>();
builder.Services.AddScoped<Blog.Posts.API.Features.Posts.ListPostsByTag.Handler>();
builder.Services.AddScoped<Blog.Posts.API.Features.Posts.ListPostsByCategory.Handler>();
builder.Services.AddScoped<Blog.Posts.API.Features.Posts.ListRecentPosts.Handler>();
builder.Services.AddScoped<Blog.Posts.API.Features.Posts.ListTags.Handler>();
builder.Services.AddScoped<Blog.Posts.API.Features.Posts.GetPostById.Handler>();
builder.Services.AddScoped<Blog.Posts.API.Features.Posts.AdminListPosts.Handler>();
builder.Services.AddScoped<Blog.Posts.API.Features.Posts.GetAdminPostById.Handler>();
builder.Services.AddScoped<Blog.Posts.API.Features.Posts.CreateAdminPost.Handler>();
builder.Services.AddScoped<IValidator<Blog.Posts.API.Features.Posts.CreateAdminPost.Request>, Blog.Posts.API.Features.Posts.CreateAdminPost.Validator>();
builder.Services.AddScoped<Blog.Posts.API.Features.Posts.UpdateAdminPost.Handler>();
builder.Services.AddScoped<IValidator<Blog.Posts.API.Features.Posts.UpdateAdminPost.Request>, Blog.Posts.API.Features.Posts.UpdateAdminPost.Validator>();
builder.Services.AddScoped<Blog.Posts.API.Features.Posts.DeletePost.Handler>();
builder.Services.AddScoped<Blog.Posts.API.Features.Posts.CreatePost.Handler>();
builder.Services.AddScoped<IValidator<Blog.Posts.API.Features.Posts.CreatePost.Request>, Blog.Posts.API.Features.Posts.CreatePost.Validator>();
builder.Services.AddScoped<Blog.Posts.API.Features.Posts.UpdatePost.Handler>();
builder.Services.AddScoped<IValidator<Blog.Posts.API.Features.Posts.UpdatePost.Request>, Blog.Posts.API.Features.Posts.UpdatePost.Validator>();
builder.Services.AddScoped<Blog.Posts.API.Features.Categories.ListCategories.Handler>();
builder.Services.AddScoped<Blog.Posts.API.Features.Categories.AdminListCategories.Handler>();
builder.Services.AddScoped<Blog.Posts.API.Features.Categories.ListCategoryOptions.Handler>();
builder.Services.AddScoped<Blog.Posts.API.Features.Categories.GetCategoryById.Handler>();
builder.Services.AddScoped<Blog.Posts.API.Features.Categories.CreateCategory.Handler>();
builder.Services.AddScoped<IValidator<Blog.Posts.API.Features.Categories.CreateCategory.Request>, Blog.Posts.API.Features.Categories.CreateCategory.Validator>();
builder.Services.AddScoped<Blog.Posts.API.Features.Categories.UpdateCategory.Handler>();
builder.Services.AddScoped<IValidator<Blog.Posts.API.Features.Categories.UpdateCategory.Request>, Blog.Posts.API.Features.Categories.UpdateCategory.Validator>();
builder.Services.AddScoped<Blog.Posts.API.Features.Categories.DeleteCategory.Handler>();
builder.Services.AddScoped<IValidator<Blog.Posts.API.Features.Categories.DeleteCategory.Request>, Blog.Posts.API.Features.Categories.DeleteCategory.Validator>();

// in memory
//builder.Services.AddTransient<IContexto, Contexto>();
//builder.Services.AddTransient<ICategoriasRepositorio,CategoriasRepositorio>();
//builder.Services.AddTransient<IPostsRepositorio,PostsRepositorio>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => (type.FullName ?? type.Name).Replace("+", "."));
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API de Exemplo",
        Description = "Uma API de exemplo usando .NET 8"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.AddJwtSecurityDefinition();
});

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

await VerificarDBExiste(app.Services, logger);

if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

async Task VerificarDBExiste(IServiceProvider services, ILogger logger)
{
    logger.LogInformation(
        "Garantindo que o banco de dados exista na string de conexao '{connectionString}'",
        connectionString
    );

    using var scope = services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await db.Database.EnsureCreatedAsync();

    if (!await db.Categorias.AnyAsync())
    {
        var categoria = new Categoria(0, "Dapper", new List<Post>
        {
            new Post(
                0,
                "Operacoes de CRUD usando Dapper",
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

public partial class Program;

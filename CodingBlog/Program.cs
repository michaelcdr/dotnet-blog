using CodingBlog.Interfaces;
using CodingBlog.Models.Data;
using CodingBlog.Repositorios.EmMemoria;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connection = builder.Configuration["ConexaoSqlite:SqliteConnectionString"];
builder.Services.AddSqlite<AppDbContext>(connection);
builder.Services.AddTransient<ICategoriasRepositorio,CategoriasRepositorio>();
builder.Services.AddTransient<IPostsRepositorio,PostsRepositorio>();

var app = builder.Build();
//await VerificarDBExiste(app.Services, app.Logger);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


async Task VerificarDBExiste(IServiceProvider services, ILogger logger)
{
    logger.LogInformation(
        "Garantindo que o banco de dados exista e esteja na string de conexão :" +
        " '{connectionString}'", connection
    );
    using var db = services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
    await db.Database.MigrateAsync();
}
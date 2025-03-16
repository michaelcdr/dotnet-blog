using Blog.Core.Services;
using CodingBlog.Configuracoes;
using CodingBlog.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient<IBlogApiService, BlogApiService>();
builder.Services.AddScoped<ISerializerService, SerializerService>();

builder.Services.AddHttpContextAccessor();
builder.Configuration.AddUserSecrets<Program>();
builder.Services.Configure<AppSettings>(builder.Configuration);

var app = builder.Build();
 
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
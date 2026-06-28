using Blog.Core.Services;
using CodingBlog.Configuracoes;
using CodingBlog.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/admin/account/login";
        options.AccessDeniedPath = "/admin/account/login";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();

builder.Services.AddTransient<JwtAuthorizationHandler>();
builder.Services.AddHttpClient("BlogAuthClient", (services, client) =>
{
    var settings = services.GetRequiredService<Microsoft.Extensions.Options.IOptions<AppSettings>>().Value;
    client.BaseAddress = new Uri(settings.UrlAuthApi);
});
builder.Services.AddHttpClient<IAuthHttpService, AuthHttpService>((services, client) =>
{
    var settings = services.GetRequiredService<Microsoft.Extensions.Options.IOptions<AppSettings>>().Value;
    client.BaseAddress = new Uri(settings.UrlAuthApi);
});
builder.Services.AddSingleton<IBlogAuthClientTokenProvider, BlogAuthClientTokenProvider>();
builder.Services
    .AddHttpClient<IBlogApiService, BlogApiService>()
    .AddHttpMessageHandler<JwtAuthorizationHandler>();
builder.Services.AddScoped<ILocalArticleImageStorage, LocalArticleImageStorage>();
builder.Services.AddScoped<ISerializerService, SerializerService>();
builder.Services.AddScoped<CodingBlog.Areas.Admin.Features.Login.Handler>();
builder.Services.AddScoped<IValidator<CodingBlog.Areas.Admin.Features.Login.Request>, CodingBlog.Areas.Admin.Features.Login.Validator>();

builder.Services.AddHttpContextAccessor();
builder.Configuration.AddUserSecrets<Program>();
builder.Services.Configure<AppSettings>(builder.Configuration);

var app = builder.Build();
 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

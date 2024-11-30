using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Configuration;
using WEB_253503_KOTOVA.API.Services.FileServices;
using WEB_253503_KOTOVA.Data;
using WEB_253503_KOTOVA.UI.Data;
using WEB_253503_KOTOVA.UI.Extensions;
using WEB_253503_KOTOVA.UI.HelperClasses;
using WEB_253503_KOTOVA.UI.Services.CategoryService;
using WEB_253503_KOTOVA.UI.Services.ProductService;
using WEB_253503_KOTOVA.Domain.Models;
using WEB_253503_KOTOVA.UI.Models;
using Serilog;
using Serilog.Events;
using WEB_253503_KOTOVA.UI.Middleware;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddSerilog();
builder.Host.UseSerilog();
Log.Logger.Information("[Started logging...]");

var keycloakData =
builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();



builder.Services
.AddAuthentication(options =>
{
    options.DefaultScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddJwtBearer()
.AddOpenIdConnect(options =>
{
    options.Authority =
    $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
    options.ClientId = keycloakData.ClientId;
    options.ClientSecret = keycloakData.ClientSecret;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.Scope.Add("openid"); // Customize scopes as needed
    options.SaveTokens = true;
    options.RequireHttpsMetadata = false; // позволяет обращаться к локальному Keycloak по http
options.MetadataAddress = $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
});
builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));

builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.RegisterCustomServices();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>(); //////

builder.Services.Configure<UriData>(builder.Configuration.GetSection("UriData"));

// Регистрация сервисов для работы с API с использованием HttpClient
builder.Services.AddHttpClient<IProductService, ApiProductService>((serviceProvider, client) =>
{
    var uriData = serviceProvider.GetRequiredService<IOptions<UriData>>().Value;
    client.BaseAddress = new Uri(uriData.ApiUri);
});

builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>((serviceProvider, client) =>
{
    var uriData = serviceProvider.GetRequiredService<IOptions<UriData>>().Value;
    client.BaseAddress = new Uri(uriData.ApiUri);
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseMiddleware<LoggerMiddleware>();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
//app.UseAuthentication(); 
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

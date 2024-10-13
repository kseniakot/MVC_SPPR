using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WEB_253503_KOTOVA.API.Services.FileServices;
using WEB_253503_KOTOVA.Data;
using WEB_253503_KOTOVA.UI.Data;
using WEB_253503_KOTOVA.UI.Extensions;
using WEB_253503_KOTOVA.UI.Services.CategoryService;
using WEB_253503_KOTOVA.UI.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.RegisterCustomServices();
builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

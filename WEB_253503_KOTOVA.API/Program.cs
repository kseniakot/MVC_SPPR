using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WEB_253503_KOTOVA.API.Data;
using WEB_253503_KOTOVA.API.Models;
using WEB_253503_KOTOVA.API.Services.CategoryServices;
using WEB_253503_KOTOVA.API.Services.ProductServices;

var builder = WebApplication.CreateBuilder(args);
var authServer = builder.Configuration
.GetSection("AuthServer")
.Get<AuthServerData>();
// Добавить сервис аутентификации
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
{
    // Адрес метаданных конфигурации OpenID
    o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";
    // Authority сервера аутентификации
    o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";
    // Audience для токена JWT
    o.Audience = "account";
    // Запретить HTTPS для использования локальной версии Keycloak
    // В рабочем проекте должно быть true
    o.RequireHttpsMetadata = false;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWasm",
        policy => policy.WithOrigins("https://localhost:7091") 
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});


// Add services to the container.

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
});

builder.Services.AddControllers();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowBlazorWasm");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

await DbInitializer.SeedData(app);


app.MapControllers();

app.Run();

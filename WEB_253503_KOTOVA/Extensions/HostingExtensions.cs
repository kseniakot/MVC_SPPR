using Microsoft.Extensions.Options;
using WEB_253503_KOTOVA.API.Services.FileServices;
using WEB_253503_KOTOVA.UI.Data;
using WEB_253503_KOTOVA.UI.HelperClasses;
using WEB_253503_KOTOVA.UI.Services.Authentification;
using WEB_253503_KOTOVA.UI.Services.CategoryService;
using WEB_253503_KOTOVA.UI.Services.ProductService;

namespace WEB_253503_KOTOVA.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(
        this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
          //  builder.Services.AddScoped<IProductService, MemoryProductService>();
            var apiUri = builder.Configuration.GetSection("UriData").GetValue<string>("ApiUri");
            builder.Services.AddHttpClient<IFileService, ApiFileService>(opt => opt.BaseAddress = new Uri($"{apiUri}Files"));
            builder.Services.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
            builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();
        }
    }
}

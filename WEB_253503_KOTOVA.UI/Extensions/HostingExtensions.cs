using WEB_253503_KOTOVA.UI.Services.CategoryService;

namespace WEB_253503_KOTOVA.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(
        this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
        }
    }
}

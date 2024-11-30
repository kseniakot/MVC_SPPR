using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WEB_253503_KOTOVA.BlazorWasm;
using WEB_253503_KOTOVA.BlazorWasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    // For more information, see https://aka.ms/blazor-standalone-auth
    builder.Configuration.Bind("Keycloak", options.ProviderOptions);
    options.UserOptions.NameClaim = "preferred_username";
});

builder.Services.AddScoped<IDataService, DataService>();

var apiBaseAddress = builder.Configuration["UriData:ApiUri"] ?? throw new InvalidOperationException("API URL not configured");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(apiBaseAddress)
});



await builder.Build().RunAsync();

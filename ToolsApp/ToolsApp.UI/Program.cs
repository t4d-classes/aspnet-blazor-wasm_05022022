using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ToolsApp.UI;

using ToolsApp.Core.Interfaces.UI;
using ToolsApp.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("ToolsApp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ToolsApp.ServerAPI"));

builder.Services.AddApiAuthorization()
    .AddAccountClaimsPrincipalFactory<CustomUserFactory>();

// builder.Services.AddScoped(sp =>
//   new HttpClient {
//     BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
//   });

builder.Services.AddScoped<IColorsData, ColorsData>();
builder.Services.AddScoped<ICarsData, CarsData>();

await builder.Build().RunAsync();

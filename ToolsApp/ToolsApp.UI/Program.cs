using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ToolsApp.UI;

using ToolsApp.Core.Interfaces.UI;
using ToolsApp.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IColorsData, ColorsData>();

builder.Services.AddScoped(sp =>
  new HttpClient {
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
  });

await builder.Build().RunAsync();

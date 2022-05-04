using Serilog;
using Serilog.Events;
using Microsoft.EntityFrameworkCore;

using ToolsApp.Api.Exceptions;
using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Data;

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();
  

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog();

builder.Services.AddSqlServer<ToolsAppContext>(
  builder.Configuration.GetConnectionString("App")
);


if (builder.Configuration["ColorsData"] == "memory")
{
  builder.Services.AddSingleton<IColorsData, ColorsInMemoryData>();
}
else
{
  builder.Services.AddScoped<IColorsData, ColorsSqlServerData>();
}

if (builder.Configuration["CarsData"] == "memory")
{
  builder.Services.AddSingleton<ICarsData, CarsInMemoryData>();
}
else
{
  builder.Services.AddScoped<ICarsData, CarsSqlServerData>();
}


builder.Services.AddControllers(options => {
  options.Filters.Add<HttpResponseExceptionFilter>();
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Serilog;
using Serilog.Events;

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

builder.Services.AddSingleton<IColorsData, ColorsInMemoryData>();

builder.Services.AddControllers();
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

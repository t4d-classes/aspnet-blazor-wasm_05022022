using Serilog;
using Serilog.Events;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Reflection;
using Microsoft.Extensions.Options;


using ToolsApp.Api.Exceptions;
using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Data;
using Swashbuckle.AspNetCore.SwaggerGen;

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();
  
try
{
  var devOrigins = "devOrigins";

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

  builder.Services.AddCors(options => {
    options.AddPolicy(name: devOrigins, builder => {
      builder.WithOrigins("https://localhost:7175")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
  });

  builder.Services.AddControllers(options => {
    options.Filters.Add<HttpResponseExceptionFilter>();
  });

  builder.Services.AddApiVersioning(config => {

    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;

  });

  builder.Services.AddVersionedApiExplorer(options => {

    options.GroupNameFormat = "'v'VVV"; // major[.minor][-status]
    options.SubstituteApiVersionInUrl = true;

  });

  // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  builder.Services.AddEndpointsApiExplorer();

  builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

  builder.Services.AddSwaggerGen(options => {

    options.OperationFilter<SwaggerDefaultValues>();

    var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
    options.IncludeXmlComments(filePath);

  });

  var app = builder.Build();

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger(options => {
      options.RouteTemplate = "api-docs/{documentName}/docs.json";
    });

    app.UseSwaggerUI(options => {

      var provider = app.Services.GetService<IApiVersionDescriptionProvider>();

      if (provider is not null)
      {
        options.RoutePrefix = "api-docs";
        foreach(var description in provider.ApiVersionDescriptions) {
          options.SwaggerEndpoint(
            $"/api-docs/{description.GroupName}/docs.json",
            description.GroupName.ToUpperInvariant()
          );
        }
      }

    });

    app.UseCors(devOrigins);
  }

  app.UseHttpsRedirection();

  app.UseAuthorization();

  app.MapControllers();

  app.Run();

}
catch(Exception exc) 
{
  Log.Fatal(exc, "Host terminated unexpectedly");
}
finally
{
  Log.CloseAndFlush();
}

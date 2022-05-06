using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;


using Duende.IdentityServer.Services;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;

using ToolsApp.Api.Exceptions;
using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Data;
using ToolsApp.Data.Models;
using Microsoft.OpenApi.Models;

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
  builder.Services.AddDatabaseDeveloperPageExceptionFilter();

  builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
      .AddRoles<IdentityRole>()
      .AddEntityFrameworkStores<ToolsAppContext>();

  builder.Services.AddIdentityServer()
      .AddApiAuthorization<ApplicationUser, ToolsAppContext>(options => {
          options.IdentityResources["openid"].UserClaims.Add("name");
          options.ApiResources.Single().UserClaims.Add("name");
          options.IdentityResources["openid"].UserClaims.Add("role");
          options.ApiResources.Single().UserClaims.Add("role");
      });


  builder.Services.AddTransient<IProfileService, ProfileService>();

  JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

  builder.Services.AddAuthentication()
      .AddIdentityServerJwt();



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

  builder.Services.AddControllersWithViews(options => {
    options.Filters.Add<HttpResponseExceptionFilter>();
  });
  builder.Services.AddRazorPages();


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
    options.OperationFilter<AddCustomHeaderParameter>();

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
          {
            Description = @"JWT Authorization header using the Bearer scheme. 
                          Enter 'Bearer' [space] and then your token in the text input below.
                          Example: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
          });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });    

    var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
    options.IncludeXmlComments(filePath);

  });

  var app = builder.Build();

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();


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
  } else {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
  }

  app.UseHttpsRedirection();

  app.UseBlazorFrameworkFiles();
  app.UseStaticFiles();

  app.UseRouting();

  app.UseIdentityServer();
  app.UseAuthentication();
  app.UseAuthorization();


  app.MapRazorPages();
  app.MapControllers();
  app.MapFallbackToFile("index.html");

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

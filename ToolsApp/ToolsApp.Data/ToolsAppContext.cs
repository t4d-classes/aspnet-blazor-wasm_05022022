using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Duende.IdentityServer.EntityFramework.Options;

using ToolsApp.Data.Models;

namespace ToolsApp.Data;

public class ToolsAppContext: ApiAuthorizationDbContext<ApplicationUser>
{

  public ToolsAppContext(
    DbContextOptions<ToolsAppContext> options,
    IOptions<OperationalStoreOptions> operationalStoreOptions)
    : base(options, operationalStoreOptions) {

      Database.Migrate();

    }

  public DbSet<Color>? Colors { get; set; }

  public DbSet<Car>? Cars { get; set; }
}
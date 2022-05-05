using Microsoft.EntityFrameworkCore;
using ToolsApp.Data.Models;

namespace ToolsApp.Data;

public class ToolsAppContext: DbContext
{

  public ToolsAppContext(DbContextOptions<ToolsAppContext> options)
    : base(options) { }

  public DbSet<Color>? Colors { get; set; }

  public DbSet<Car>? Cars { get; set; }
}
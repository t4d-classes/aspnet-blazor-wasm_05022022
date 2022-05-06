using ToolsApp.Core.Interfaces.Models;
using ToolsApp.Core.Validators;

namespace ToolsApp.Models;

public class NewCar: INewCar {

  public string? Make { get; set; }

  public string? Model { get; set; }

  [MinCarYear]
  public int? Year { get; set; }

  public string? Color { get; set; }

  public decimal? Price { get; set; }
}

public class Car: NewCar, ICar {

  public int Id { get; set; }
  
}
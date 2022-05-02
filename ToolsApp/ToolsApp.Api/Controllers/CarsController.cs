using Microsoft.AspNetCore.Mvc;
using ToolsApp.Models;

namespace ToolsApp.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class CarsController: ControllerBase
{

  [HttpGet]
  public ActionResult<IEnumerable<Car>> Get()
  {
    var cars = new List<Car> {
      new() {
        Id=1, Make="Ford", Model="Fusion Hybrid",
        Year=2020, Color="Red", Price=45000,
      },
      new() {
        Id=2, Make="Tesla", Model="S",
        Year=2019, Color="Black", Price=120000,
      },
    };

    return Ok(cars);
  }

}
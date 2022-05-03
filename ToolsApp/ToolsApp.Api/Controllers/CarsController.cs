using Microsoft.AspNetCore.Mvc;
using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;
using ToolsApp.Models;

namespace ToolsApp.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class CarsController: ControllerBase
{
  private ILogger _logger;
  private ICarsData _carsData;

  public CarsController(ICarsData carsData, ILogger<CarsController> logger)
  {
    _carsData = carsData;
    _logger = logger;
  }

  /// <summary>
  /// Returns a list of cars
  /// </summary>
  /// <remarks>
  /// How to call:
  ///   
  ///   GET /v1/cars
  ///
  /// </remarks>
  /// <response code="200">List of cars</response>
  /// <response code="500">Errors occurred.</response>
  /// <returns>List of Cars</returns>
  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(typeof(IEnumerable<ICar>), StatusCodes.Status200OK)]
  public async Task<ActionResult<IEnumerable<Car>>> Get()
  {
    try
    {
      return Ok(await _carsData.All());
    }
    catch(Exception exc)
    {
      _logger.LogError(exc, "All cars failed.");
      throw;
    }
  }

}
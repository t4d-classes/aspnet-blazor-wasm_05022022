using Microsoft.AspNetCore.Mvc;
using ToolsApp.Api.Exceptions;
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

  /// <summary>
  /// Remove a car by id.
  /// </summary>
  /// <remarks>
  /// How to call:
  /// 
  ///     DELETE /cars/1
  ///     
  /// </remarks>
  /// <param name="carId">Id of car to remove.</param>
  /// <response code="204">No Content.</response>
  /// <response code="500">Error occurred.</response>
  /// <returns>Nothing</returns>
  [HttpDelete("{carId:int}")]
  [ProducesResponseType(typeof(ICar), StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<ICar>> RemoveCar(
    int carId
  )
  {
    try
    {
      await _carsData.Remove(carId);
      return NoContent();
    }
    catch (IndexOutOfRangeException exc)
    {
      var errorMessage = "Unable to find car to remove.";
      _logger.LogError(exc, errorMessage);
      return NotFound(errorMessage);
    }
    catch (Exception exc)
    {
      _logger.LogError(exc, "Remove car failed.");
      throw new InternalServerErrorException();
    }
  }  

}
using Microsoft.AspNetCore.Mvc;
using ToolsApp.Api.Exceptions;
using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;
using ToolsApp.Models;

namespace ToolsApp.Api.Controllers;

[Route("v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("1.1")]
[ApiController]
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
  public async Task<ActionResult<IEnumerable<Car>>> All()
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
  /// Return a car for the given id
  /// </summary>
  /// <remarks>
  /// How to call:
  ///   
  ///   GET /v1/cars/1
  ///
  /// </remarks>
  /// <param name="carId">Id of the car to retrieve</param>
  /// <response code="200">A single car</response>
  /// <response code="404">No car found for the specified id</response>
  /// <response code="500">Errors occurred.</response>
  /// <returns>Car</returns>
  [HttpGet("{carId:int}")]
  [MapToApiVersion("1.0")]
  [Produces("application/json")]
  [ProducesResponseType(typeof(ICar), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<ICar>> One(int carId)
  {
    try
    {
      var car = await _carsData.One(2);

      if (car is null) {
        var infoMessage = $"Unable to find car with id {carId}";
        _logger.LogInformation(infoMessage);
        return NotFound(infoMessage);
      } else {
        return Ok(car);
      }
    }
    catch(Exception exc)
    {
      _logger.LogError(exc, "One car failed.");
      throw new InternalServerErrorException();
    }
  }

  /// <summary>
  /// Return a car for the given id
  /// </summary>
  /// <remarks>
  /// How to call:
  ///   
  ///   GET /v1/cars/1
  ///
  /// </remarks>
  /// <param name="carId">Id of the car to retrieve</param>
  /// <response code="200">A single car</response>
  /// <response code="404">No car found for the specified id</response>
  /// <response code="500">Errors occurred.</response>
  /// <returns>Car</returns>
  [HttpGet("{carId:int}")]
  [MapToApiVersion("1.1")]
  [Produces("application/json")]
  [ProducesResponseType(typeof(ICar), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<ICar>> OneV1_1(int carId)
  {
    try
    {
      var car = await _carsData.One(1);

      if (car is null) {
        var infoMessage = $"Unable to find car with id {carId}";
        _logger.LogInformation(infoMessage);
        return NotFound(infoMessage);
      } else {
        return Ok(car);
      }
    }
    catch(Exception exc)
    {
      _logger.LogError(exc, "One car failed.");
      throw new InternalServerErrorException();
    }
  }



  /// <summary>
  /// Append a Car
  /// </summary>
  /// <remarks>
  /// How to call:
  ///   
  ///   POST /v1/cars
  ///
  ///   Request body is a JSON serialized NewCar object
  ///
  /// </remarks>
  /// <response code="201">Created car</response>
  /// <response code="400">Car was invalid.</response>
  /// <response code="500">Errors occurred.</response>
  /// <returns>Car</returns>
  [HttpPost]
  [Consumes("application/json")]
  [Produces("application/json")]
  [ProducesResponseType(typeof(ICar), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<ICar>> Append(
    [FromBody] NewCar newCar
  )
  {
    try
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var car = await _carsData.Append(newCar);
      return Created($"/v1/cars/{car.Id}", car);
    }
    catch(Exception exc)
    {
      _logger.LogError(exc, "One car failed.");
      throw new InternalServerErrorException();
    }    
  }

  /// <summary>
  /// Append a Car
  /// </summary>
  /// <remarks>
  /// How to call:
  ///   
  ///   POST /v1/cars
  ///
  ///   Request body is a JSON serialized NewCar object
  ///
  /// </remarks>
  /// <response code="201">Created car</response>
  /// <response code="400">Car was invalid.</response>
  /// <response code="500">Errors occurred.</response>
  /// <returns>Car</returns>
  [HttpPut("{carId:int}")]
  [Consumes("application/json")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult> Replace(int carId, [FromBody] Car car
  )
  {
    try
    {
      if (!ModelState.IsValid || car is null)
      {
        return BadRequest();
      }

      if (carId != car.Id)
      {
        return BadRequest("car ids do not match");
      }

      await _carsData.Replace(car);

      return NoContent();
    }
    catch (IndexOutOfRangeException exc)
    {
      var errorMessage = "Unable to find car to remove.";
      _logger.LogError(exc, errorMessage);
      return NotFound(errorMessage);
    }    
    catch(Exception exc)
    {
      _logger.LogError(exc, "One car failed.");
      throw new InternalServerErrorException();
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
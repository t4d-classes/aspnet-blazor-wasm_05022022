using Microsoft.AspNetCore.Mvc;
using ToolsApp.Api.Exceptions;
using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

namespace ToolsApp.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class ColorsController: ControllerBase
{
  private ILogger _logger;
  private IColorsData _colorsData;

  public ColorsController(IColorsData colorsData, ILogger<ColorsController> logger)
  {
    _colorsData = colorsData;
    _logger = logger;
  }

  /// <summary>
  /// Returns a list of colors
  /// </summary>
  /// <remarks>
  /// How to call:
  ///   
  ///   GET /v1/colors
  ///
  /// </remarks>
  /// <response code="200">List of colors</response>
  /// <response code="500">Errors occurred.</response>
  /// <returns>List of Colors</returns>
  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(typeof(IEnumerable<IColor>), StatusCodes.Status200OK)]
  public async Task<ActionResult<IEnumerable<IColor>>> All()
  {
    try
    {
      // if you get paid by the hour
      // var result = new ObjectResult(await _colorsData.All());
      // result.StatusCode = StatusCodes.Status200OK;
      // return result;

      // if you are paid salary
      // var result = new OkObjectResult(await _colorsData.All());
      // return result;

      // if you are a volunteer
      return Ok(await _colorsData.All());
    }
    catch(Exception exc)
    {
      _logger.LogError(exc, "All colors failed.");
      throw;
    }
  }

  /// <summary>
  /// Return a color for the given id
  /// </summary>
  /// <remarks>
  /// How to call:
  ///   
  ///   GET /v1/colors/1
  ///
  /// </remarks>
  /// <param name="colorId">Id of the color to retrieve</param>
  /// <response code="200">A single color</response>
  /// <response code="404">No color found for the specified id</response>
  /// <response code="500">Errors occurred.</response>
  /// <returns>Color</returns>
  [HttpGet("{colorId:int}")]
  [Produces("application/json")]
  [ProducesResponseType(typeof(IColor), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<IColor>> One(int colorId)
  {
    try
    {
      var color = await _colorsData.One(colorId);

      if (color is null) {
        var infoMessage = $"Unable to find color with id {colorId}";
        _logger.LogInformation(infoMessage);
        return NotFound(infoMessage);
      } else {
        return Ok(color);
      }
    }
    catch(Exception exc)
    {
      _logger.LogError(exc, "One color failed.");
      throw new InternalServerErrorException();
    }
  }

  /// <summary>
  /// Remove a color by id.
  /// </summary>
  /// <remarks>
  /// How to call:
  /// 
  ///     DELETE /colors/1
  ///     
  /// </remarks>
  /// <param name="colorId">Id of color to remove.</param>
  /// <response code="204">No Content.</response>
  /// <response code="500">Error occurred.</response>
  /// <returns>Nothing</returns>
  [HttpDelete("{colorId:int}")]
  [ProducesResponseType(typeof(IColor), StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<IColor>> RemoveColor(
    int colorId
  )
  {
    try
    {
      await _colorsData.Remove(colorId);
      return NoContent();
    }
    catch (IndexOutOfRangeException exc)
    {
      var errorMessage = "Unable to find color to remove.";
      _logger.LogError(exc, errorMessage);
      return NotFound(errorMessage);
    }
    catch (Exception exc)
    {
      _logger.LogError(exc, "Remove color failed.");
      throw new InternalServerErrorException();
    }
  }  

}
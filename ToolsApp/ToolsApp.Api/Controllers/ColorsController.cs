using Microsoft.AspNetCore.Mvc;
using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;
using ToolsApp.Models;

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
  public async Task<ActionResult<IEnumerable<Color>>> Get()
  {
    try
    {
      return Ok(await _colorsData.All());
    }
    catch(Exception exc)
    {
      _logger.LogError(exc, "All colors failed.");
      throw;
    }
  }

}
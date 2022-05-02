using Microsoft.AspNetCore.Mvc;
using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Models;

namespace ToolsApp.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class ColorsController: ControllerBase
{
  private IColorsData _colorsData;

  public ColorsController(IColorsData colorsData)
  {
    _colorsData = colorsData;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<Color>>> Get()
  {
    return Ok(await _colorsData.All());
  }

}
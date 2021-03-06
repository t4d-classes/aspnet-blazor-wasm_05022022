using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolsApp.Models;

namespace ToolsApp.Api.Controllers;

[Route("v{version:apiVersion}/[controller]")]
[Authorize(Roles="User")]
[ApiVersion("1.0")]
[ApiController]
public class HelloWorldController: ControllerBase
{

  [HttpGet]
  public ActionResult<Message> Get()
  {
    return Ok(new Message { Contents = "Hello, World!" });
  }

}
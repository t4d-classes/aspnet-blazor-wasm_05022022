using Microsoft.AspNetCore.Mvc;
using ToolsApp.Models;

namespace ToolsApp.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class HelloWorldController: ControllerBase
{

  [HttpGet]
  public ActionResult<Message> Get()
  {
    return Ok(new Message { Contents = "Hello, World!" });
  }

}
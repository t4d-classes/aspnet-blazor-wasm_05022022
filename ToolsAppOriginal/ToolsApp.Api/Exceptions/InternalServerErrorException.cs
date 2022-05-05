namespace ToolsApp.Api.Exceptions;

public class InternalServerErrorException: HttpResponseException
{
  public InternalServerErrorException(): base(
    StatusCodes.Status500InternalServerError,
    "Internal Server Error"
  ) { }

}
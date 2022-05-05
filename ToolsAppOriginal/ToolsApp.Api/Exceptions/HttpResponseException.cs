namespace ToolsApp.Api.Exceptions;

public class HttpResponseException: Exception
{
  public int StatusCode { get; }

  public object? Value { get; }

  public HttpResponseException(int statusCode, object? value = null)
  {
    StatusCode = statusCode;
    Value = value;
  }

}
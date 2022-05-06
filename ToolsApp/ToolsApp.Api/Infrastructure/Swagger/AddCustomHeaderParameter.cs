using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AddCustomHeaderParameter : IOperationFilter
{
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "SomeCustomHeader",
            Description = "An example custom header field",
            In = ParameterLocation.Header,
            Schema = new OpenApiSchema {
              Type="string"
            },
            Required = false
        });
  }
}
# Exercise 3

1. Implement the cars endpoint for getting one car by id. The implementation is very similar to the get one color by id endpoint.

2. Implement the delete car endpoint. The endpoint should handle the following URL:

DELETE /v1/colors/1

To delete something from a list in C# is similar to this:

```csharp
var colorIndex = _colors.FindIndex(c => c.Id == colorId);

_colors.RemoveAt(colorIndex);
```

3. Ensure both endpoints work in Swagger.
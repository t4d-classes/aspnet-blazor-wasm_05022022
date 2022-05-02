# Exercise 1

1. In the appropriate project and folder, define a new Car model with the following properties:

- Id: int
- Make: string
- Model: string
- Year: int
- Color: string
- Price: decimal

2. In the appropriate project and folder, add a new controller to create a new REST API endpoint to manage car resources.

3. Add a controller action method to return a collection of two cars. Hard code the car data within the action method similar to how the message model was hard coded and returned. Review the weather forecast to see how collections of data are returned. Below is some example code on how to create a collection of messages. Follow a similar pattern for cars.

```csharp
var messages = new List<Message>()
{
  new() { Content= "A" },
  new() { Content= "B" },
};
```

4. Run the application, load the collection of cars from the endpoint using Swagger and the browser's location bar.
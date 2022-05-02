namespace ToolsApp.Core.Interfaces.Models;

public interface IColor
{
  int? Id {get; set;}
  string? Name {get; set;}
  string? Hexcode { get; set; }
}
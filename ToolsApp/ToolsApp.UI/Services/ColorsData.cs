using ToolsApp.Core.Interfaces.Models;
using ToolsApp.Core.Interfaces.UI;

using ToolsApp.Models;

namespace ToolsApp.UI.Services;

public class ColorsData : IColorsData
{
  private List<Color> _colors = new List<Color>() {
    new() { Id=1, Name="red", Hexcode="ff0000"},
    new() { Id=2, Name="green", Hexcode="00ff00"},
    new() { Id=3, Name="blue", Hexcode="0000ff"},
  };

  public Task<IEnumerable<IColor>> All()
  {
    return Task.FromResult(_colors.Cast<IColor>());
  }

  public Task<IColor> Append(INewColor newColor)
  {
    var color = new Color {
      Id=_colors.Any() ? _colors.Max(c => c.Id) + 1 : 1,
      Name=newColor.Name,
      Hexcode=newColor.Hexcode
    };

    _colors.Add(color);

    return Task.FromResult(color as IColor);
  }
}
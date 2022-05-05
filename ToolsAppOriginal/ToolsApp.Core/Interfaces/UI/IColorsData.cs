using ToolsApp.Core.Interfaces.Models;

namespace ToolsApp.Core.Interfaces.UI;

public interface IColorsData {
  
  Task<IEnumerable<IColor>> All();

  Task<IColor> Append(INewColor newColor);
}
using ToolsApp.Core.Interfaces.Models;

namespace ToolsApp.Core.Interfaces.UI;

public interface ICarsData {
  
  Task<IEnumerable<ICar>> All();

  Task<ICar?> One(int carId);

  Task<ICar> Append(INewCar newCar);

  Task Replace(ICar car);

  Task Remove(int carId);
}
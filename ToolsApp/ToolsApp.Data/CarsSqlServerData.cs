using AutoMapper;
using Microsoft.EntityFrameworkCore;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

using CarModel = ToolsApp.Models.Car;
using CarDataModel = ToolsApp.Data.Models.Car;

namespace ToolsApp.Data;

public class CarsSqlServerData : ICarsData
{
  private ToolsAppContext _toolsAppContext;

  private IMapper _mapper;

  public CarsSqlServerData(ToolsAppContext toolsAppContext) 
  {
    _toolsAppContext = toolsAppContext;

    var mapperConfig = new MapperConfiguration(config => {
      config.CreateMap<INewCar, CarDataModel>();
      config.CreateMap<ICar, CarDataModel>();
      config.CreateMap<CarDataModel, CarModel>().ReverseMap();
    });

    _mapper = mapperConfig.CreateMapper();    
  }
  public async Task<IEnumerable<ICar>> All()
  {
    if (_toolsAppContext.Cars is null) {
      throw new NullReferenceException(
        "ToolsAppContext.Cars cannot be null");
    }

    return await _toolsAppContext.Cars
      .Select(car => _mapper.Map<CarDataModel, CarModel>(car))
      .AsNoTracking()
      .ToListAsync();
  }

  public async Task<ICar> Append(INewCar car)
  {
    var carDataModel = _mapper.Map<INewCar, CarDataModel>(car);

      await _toolsAppContext.AddAsync(carDataModel);
      await _toolsAppContext.SaveChangesAsync();
      _toolsAppContext.ChangeTracker.Clear();

      return _mapper.Map<CarDataModel, CarModel>(carDataModel);
  }

  public async Task<ICar?> One(int carId)
  {
    if (_toolsAppContext.Cars is null) {
        throw new NullReferenceException(
          "ToolAppsContext.Car cannot be null.");
      }

    return await _toolsAppContext.Cars
      .Where(c => c.Id == carId)
      .Select(car => _mapper.Map<CarDataModel, CarModel>(car))
      .AsNoTracking()
      .FirstAsync();    
  }

  public async Task Remove(int carId)
  {
    if (_toolsAppContext.Cars is null) {
        throw new NullReferenceException(
          "ToolAppsContext.Car cannot be null.");
      }

      var carDataModel = await _toolsAppContext.Cars.FindAsync(carId);

      if (carDataModel is null) {
        throw new NullReferenceException(
          $"Unable to find car with id {carId}");
      }

      _toolsAppContext.Cars.Remove(carDataModel);
      await _toolsAppContext.SaveChangesAsync();
  }

  public async Task Replace(ICar car)
  {
     if (_toolsAppContext.Cars is null) {
        throw new NullReferenceException(
          "ToolAppsContext.Car cannot be null.");
      }

      var oldCarDataModel = await _toolsAppContext.Cars
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == car.Id);

      if (oldCarDataModel is null)
      {
        throw new NullReferenceException($"Unable to find car {car.Id}");
      }

      var carDataModel = _mapper.Map<ICar, CarDataModel>(car);

      _toolsAppContext.Update(carDataModel);
      await _toolsAppContext.SaveChangesAsync();
  }
}
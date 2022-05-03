using AutoMapper;

using CarModel = ToolsApp.Models.Car;
using CarDataModel = ToolsApp.Data.Models.Car;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

namespace ToolsApp.Data;

public class CarsInMemoryData : ICarsData
{
  private IMapper _mapper;

  private List<CarDataModel> _cars = new() {
      new() {
        Id=1, Make="Ford", Model="Fusion Hybrid",
        Year=2020, Color="Red", Price=45000,
      },
      new() {
        Id=2, Make="Tesla", Model="S",
        Year=2019, Color="Black", Price=120000,
      },
    };

  public CarsInMemoryData()
  {
    var mapperConfig = new MapperConfiguration(config => {
      config.CreateMap<CarDataModel, CarModel>().ReverseMap();
    });

    _mapper = mapperConfig.CreateMapper();
  }

  public Task<IEnumerable<ICar>> All()
  {
    return Task.FromResult(
      _cars
        .Select(carDataModel =>
          _mapper.Map<CarDataModel, CarModel>(carDataModel))
        .AsEnumerable<ICar>()
    );
  }

  public Task<ICar?> One(int carId)
  {
    return Task.FromResult(_cars
      .Where(c => c.Id == carId)
      .Select(c => _mapper.Map<CarDataModel, CarModel>(c))
      .Cast<ICar>()
      .SingleOrDefault());
  }

  public Task Remove(int carId)
  {
    var carIndex = _cars.FindIndex(c => c.Id == carId);

    if (carIndex == -1)
    {
      throw new IndexOutOfRangeException("Car not found");
    }

    _cars.RemoveAt(carIndex);

    return Task.CompletedTask;
  }   
}

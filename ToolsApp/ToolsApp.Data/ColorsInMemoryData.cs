using AutoMapper;

using ColorModel = ToolsApp.Models.Color;
using ColorDataModel = ToolsApp.Data.Models.Color;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

namespace ToolsApp.Data;

public class ColorsInMemoryData : IColorsData
{
  private IMapper _mapper;

  private List<ColorDataModel> _colors = new() {
    new() { Id=1, Name="red", Hexcode="ff0000" },
    new() { Id=2, Name="green", Hexcode="00ff00" },
    new() { Id=3, Name="blue", Hexcode="0000ff" },
  };

  public ColorsInMemoryData()
  {
    var mapperConfig = new MapperConfiguration(config => {
      config.CreateMap<ColorDataModel, ColorModel>().ReverseMap();
    });

    _mapper = mapperConfig.CreateMapper();
  }

  public Task<IEnumerable<IColor>> All()
  {
    return Task.FromResult(
      _colors
        .Select(colorDataModel =>
          _mapper.Map<ColorDataModel, ColorModel>(colorDataModel))
        .AsEnumerable<IColor>()
    );
  }
}

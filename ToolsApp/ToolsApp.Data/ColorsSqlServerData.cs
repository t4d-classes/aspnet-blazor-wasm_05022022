using AutoMapper;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

using ColorModel = ToolsApp.Models.Color;
using ColorDataModel = ToolsApp.Data.Models.Color;
using Microsoft.EntityFrameworkCore;

namespace ToolsApp.Data;

public class ColorsSqlServerData : IColorsData
{
  private ToolsAppContext _toolsAppContext;

  private IMapper _mapper;

  public ColorsSqlServerData(ToolsAppContext toolsAppContext) 
  {
    _toolsAppContext = toolsAppContext;

    var mapperConfig = new MapperConfiguration(config => {
      config.CreateMap<INewColor, ColorDataModel>();
      config.CreateMap<IColor, ColorDataModel>();
      config.CreateMap<ColorDataModel, ColorModel>().ReverseMap();
    });

    _mapper = mapperConfig.CreateMapper();    
  }
  public async Task<IEnumerable<IColor>> All()
  {
    if (_toolsAppContext.Colors is null) {
      throw new NullReferenceException(
        "ToolsAppContext.Colors cannot be null");
    }

    return await _toolsAppContext.Colors
      .Select(color => _mapper.Map<ColorDataModel, ColorModel>(color))
      .AsNoTracking()
      .ToListAsync();
  }

  public Task<IColor> Append(INewColor color)
  {
    throw new NotImplementedException();
  }

  public Task<IColor?> One(int colorId)
  {
    throw new NotImplementedException();
  }

  public Task Remove(int colorId)
  {
    throw new NotImplementedException();
  }

  public Task Replace(IColor color)
  {
    throw new NotImplementedException();
  }
}
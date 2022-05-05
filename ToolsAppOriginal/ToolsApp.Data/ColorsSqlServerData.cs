using AutoMapper;
using Microsoft.EntityFrameworkCore;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

using ColorModel = ToolsApp.Models.Color;
using ColorDataModel = ToolsApp.Data.Models.Color;

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

  public async Task<IColor> Append(INewColor color)
  {
    var colorDataModel = _mapper.Map<INewColor, ColorDataModel>(color);

      await _toolsAppContext.AddAsync(colorDataModel);
      await _toolsAppContext.SaveChangesAsync();
      _toolsAppContext.ChangeTracker.Clear();

      return _mapper.Map<ColorDataModel, ColorModel>(colorDataModel);
  }

  public async Task<IColor?> One(int colorId)
  {
    if (_toolsAppContext.Colors is null) {
        throw new NullReferenceException(
          "ToolAppsContext.Color cannot be null.");
      }

    return await _toolsAppContext.Colors
      .Where(c => c.Id == colorId)
      .Select(color => _mapper.Map<ColorDataModel, ColorModel>(color))
      .AsNoTracking()
      .FirstAsync();    
  }

  public async Task Remove(int colorId)
  {
    if (_toolsAppContext.Colors is null) {
        throw new NullReferenceException(
          "ToolAppsContext.Car cannot be null.");
      }

      var colorDataModel = await _toolsAppContext.Colors.FindAsync(colorId);

      if (colorDataModel is null) {
        throw new NullReferenceException(
          $"Unable to find color with id {colorId}");
      }

      _toolsAppContext.Colors.Remove(colorDataModel);
      await _toolsAppContext.SaveChangesAsync();
  }

  public async Task Replace(IColor color)
  {
     if (_toolsAppContext.Colors is null) {
        throw new NullReferenceException(
          "ToolAppsContext.Car cannot be null.");
      }

      var oldColorDataModel = await _toolsAppContext.Colors
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == color.Id);

      if (oldColorDataModel is null)
      {
        throw new NullReferenceException($"Unable to find color {color.Id}");
      }

      var colorDataModel = _mapper.Map<IColor, ColorDataModel>(color);

      _toolsAppContext.Update(colorDataModel);
      await _toolsAppContext.SaveChangesAsync();
  }
}
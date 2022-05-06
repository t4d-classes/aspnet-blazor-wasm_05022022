

namespace ToolsApp.UI.Services;

public abstract class BaseData {

  protected string _baseUrl = "";

  protected string collectionUrl() => _baseUrl;

  protected string elementUrl(int elementId) =>
    $"{_baseUrl}/{Uri.EscapeDataString(elementId.ToString())}";

}


public class ConfigurationAdapter : IConfigurationAdapter {

  public string GetConnectionString(string name) {
    var builder = App.GetBuilder();
    var result = builder.Configuration.GetConnectionString(name);
    return result == null ? String.Empty : result;
  }

  public T GetValue<T>(string name)
  {
    var builder = App.GetBuilder();
    var result = builder.Configuration.GetValue<T>(name);
    return result;
  }
}
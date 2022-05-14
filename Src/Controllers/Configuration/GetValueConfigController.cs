


namespace Controller
{
  public class GetValueConfigController
  {
    private readonly IConfigurationAdapter _configuration;

    public GetValueConfigController(IConfigurationAdapter configuration)
    {
      _configuration = configuration;
    }

    /// <summary>
    /// key name in configuration
    /// </summary>
    public T Execute<T>(string name)
    {
      return _configuration.GetValue<T>(name);
    }
  }
}
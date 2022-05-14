


namespace Controller
{
  public class GetConnectionStringController
  {
    private readonly IConfigurationAdapter _configuration;

    public GetConnectionStringController(IConfigurationAdapter configuration)
    {
      _configuration = configuration;
    }

    /// <summary>
    /// key name in configuration
    /// </summary>
    public string Execute(string name)
    {
      return _configuration.GetConnectionString(name);
    }
  }
}

using Controller;

namespace Factory
{
  public class GetConnectionStringFactory {
    
    public static GetConnectionStringController Creator() {

      var configurationAdapter = new ConfigurationAdapter();

      return new GetConnectionStringController(configurationAdapter);
    }
  }
}
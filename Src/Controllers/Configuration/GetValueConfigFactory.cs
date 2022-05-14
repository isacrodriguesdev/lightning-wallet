
using Controller;

namespace Factory
{
  public class GetValueConfigFactory {
    
    public static GetValueConfigController Creator() {

      var configurationAdapter = new ConfigurationAdapter();

      return new GetValueConfigController(configurationAdapter);
    }
  }
}
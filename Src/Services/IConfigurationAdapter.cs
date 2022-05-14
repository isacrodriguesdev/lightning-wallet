
public interface IConfigurationAdapter
{
  string GetConnectionString(string name);
  T GetValue<T>(string name);
}

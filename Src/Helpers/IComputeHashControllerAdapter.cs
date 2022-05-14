
namespace Helper
{
  public interface IComputeHashControllerAdapter {
    string GenerateSHA256(string data);
    string GenerateHMACSHA256(string data, string key);
  }
}

namespace Helper
{
  public interface IPasswordControllerAdapter {
    string GeneratePassword(string password);
    bool ComparePassword(string hashPassword, string password);
  }
}
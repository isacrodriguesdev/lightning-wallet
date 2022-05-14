
namespace Helper
{
  public class BCryptoControllerAdapter : IPasswordControllerAdapter
  {
    public bool ComparePassword(string hashPassword, string password)
    {
      return BCrypt.Net.BCrypt.Verify(password, hashPassword);
    }

    public string GeneratePassword(string password)
    {
      string passwordSalt = BCrypt.Net.BCrypt.GenerateSalt(10);
      string passwordHash = BCrypt.Net.BCrypt.HashPassword(password, passwordSalt);

      return passwordHash;
    }
  }
}
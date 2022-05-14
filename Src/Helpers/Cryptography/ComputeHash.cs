using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Helper
{

  public class ComputeHash : IComputeHashControllerAdapter
  {
    public string GenerateSHA256(string data)
    {
      var mySha256 = System.Security.Cryptography.SHA256.Create();
      var hash = new StringBuilder();

      byte[] rawDataBytes = Encoding.UTF8.GetBytes(data);
      byte[] computedHash = mySha256.ComputeHash(rawDataBytes);

      for (int i = 0; i < computedHash.Length; i++)
      {
        hash.Append(computedHash[i].ToString("x2"));
      }
      return hash.ToString().ToLower();
    }

    public string GenerateHMACSHA256(string data, string secretKey)
    {
      HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
      var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));
      return Convert.ToHexString(signature).ToLower();
    }
  }
}
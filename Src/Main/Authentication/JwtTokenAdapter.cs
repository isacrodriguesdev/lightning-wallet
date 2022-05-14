
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Model;
using Factory;
using Microsoft.IdentityModel.Tokens;


public class JwtTokenAdapter : ITokenControllerAdapter
{
  public string GenerateToken(User user)
  {
    var getValueConfig = GetValueConfigFactory.Creator();

    var tokenHandler = new JwtSecurityTokenHandler();
    byte[] securityKey = Encoding.ASCII.GetBytes(getValueConfig.Execute<string>("SecurityKey"));

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new Claim[] {
        new Claim(ClaimTypes.NameIdentifier, user.Id)
      }),
      SigningCredentials = new SigningCredentials(
        new SymmetricSecurityKey(securityKey),
        SecurityAlgorithms.HmacSha256Signature
      ),
      Expires = DateTime.UtcNow.AddHours(48)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return "Bearer " + tokenHandler.WriteToken(token);
  }
}
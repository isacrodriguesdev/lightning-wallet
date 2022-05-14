using Controller;
using MongoDB;

namespace Factory
{
  public class LoginUserFactory
  {
    public static LoginUserController Creator()
    {
      
      var userRepositoryAdapter = new UserRepositoryAdapter();
      var bCryptoControllerAdapter = new Helper.BCryptoControllerAdapter();
      var tokenController = new JwtTokenAdapter();

      return new LoginUserController(userRepositoryAdapter, bCryptoControllerAdapter, tokenController);
    }
  }
}
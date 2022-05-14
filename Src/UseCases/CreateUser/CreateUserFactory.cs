
namespace Factory
{
  public class CreateUserFactory
  {
    public static Controller.CreateUserController Creator()
    {
      var userRepositoryAdapter = new MongoDB.UserRepositoryAdapter();
      var bCryptoControllerAdapter = new Helper.BCryptoControllerAdapter();
      var mailService = new MailService();
      var confirmationCodeRepository = new MongoDB.ConfirmationCodeRepositoryAdapter();

      return new Controller.CreateUserController(userRepositoryAdapter, bCryptoControllerAdapter, mailService, confirmationCodeRepository);
    }
  }
}
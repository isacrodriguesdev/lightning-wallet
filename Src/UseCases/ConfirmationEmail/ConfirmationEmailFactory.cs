using Adapter;
using Helper;

public class ConfirmationEmailFactory
{
  public static Controller.ConfirmationEmailController Creator()
  {
    var userRepositoryAdapter = new MongoDB.UserRepositoryAdapter();
    var jwtTokenAdapter = new JwtTokenAdapter();
    var mailService = new MailService();
    var confirmationCodeRepository = new MongoDB.ConfirmationCodeRepositoryAdapter();

    return new Controller.ConfirmationEmailController(
      userRepositoryAdapter,
      confirmationCodeRepository,
      jwtTokenAdapter
    );
  }
}

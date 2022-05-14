using Protocol;
using Model;


namespace Controller
{
  public class ConfirmationEmailController : IHttpController<ConfirmationCodeHttpRequest>
  {

    private readonly IUserRepository _userRepository;
    private readonly IConfirmationCodeRepository _confirmationCodeRepository;
    private readonly ITokenControllerAdapter _tokenController;

    public ConfirmationEmailController(IUserRepository userRepository, IConfirmationCodeRepository confirmationCodeRepository, ITokenControllerAdapter tokenController)
    {
      _userRepository = userRepository;
      _confirmationCodeRepository = confirmationCodeRepository;
      _tokenController = tokenController;
    }

    public async Task<HttpResponsePacket> Execute(HttpRequestPacket<ConfirmationCodeHttpRequest> request)
    {
      ConfirmationCode confirmation;

      if (request.Data.Id != null)
      { confirmation = await _confirmationCodeRepository.FindCode(request.Data.Id); }
      else
      { confirmation = await _confirmationCodeRepository.FindCode(request.Data.Code); }

      if (confirmation == null)
      {
        return new HttpResponsePacket
        {
          Data = new { Error = "Invalid code" },
          StatusCode = 400
        };
      }

      if (DateTime.UtcNow.CompareTo(confirmation.Expiry) == 1)
      {
        // implement resend email confirmation
        return new HttpResponsePacket
        {
          Data = new { Error = "Expired code" },
          StatusCode = 400
        };
      }

      User user = await _userRepository.GetUnverifiedUserById(confirmation.UserId);

      if (user == null)
      {
        return new HttpResponsePacket
        {
          Data = new { Error = "Account already verified" },
          StatusCode = 400
        };
      }

      await _userRepository.UpdateConfirmedAccount(user.Id);

      return new HttpResponsePacket
      {
        Data = new
        {
          Token = _tokenController.GenerateToken(user),
        },
        StatusCode = 200
      };
    }
  }
}
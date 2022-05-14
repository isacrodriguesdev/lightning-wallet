using Model;
using Protocol;
using Helper;

namespace Controller
{
  public class CreateUserController : IHttpController<UserRegisterHttpRequest>
  {
    private readonly IUserRepository _userRepository;
    private readonly IPasswordControllerAdapter _passwordControllerAdapter;
    private readonly IConfirmationCodeRepository _confirmationCodeRepository;
    private readonly IMailService _mailService;

    public CreateUserController(
      IUserRepository userRepository,
      IPasswordControllerAdapter passwordControllerAdapter,
      IMailService mailService,
      IConfirmationCodeRepository confirmationCodeRepository
      )
    {
      _userRepository = userRepository;
      _passwordControllerAdapter = passwordControllerAdapter;
      _mailService = mailService;
      _confirmationCodeRepository = confirmationCodeRepository;
    }

    public async Task<HttpResponsePacket> Execute(HttpRequestPacket<UserRegisterHttpRequest> httpRequest)
    {
      var resultUser = await _userRepository.GetByEmail(httpRequest.Data.Email);

      if (resultUser != null)
      {
        return new HttpResponsePacket()
        {
          StatusCode = 500,
          Data = new { Error = "Internal Server Error" }
        };
      }

      // has incorrect, implement interface and inject depedency
      var userRegisterValidator = new Validator.UserRegisterHttpRequestValidator();
      var validationResult = userRegisterValidator.Validate(httpRequest.Data);

      if(validationResult.IsValid == false) {
        return new HttpResponsePacket()
        {
          Data = new { Errors = validationResult.Errors },
          StatusCode = 400
        };
      }

      User user = new User {
        Name = httpRequest.Data.Name,
        Username = httpRequest.Data.Username,
        PhoneNumber = httpRequest.Data.PhoneNumber,
        Email = httpRequest.Data.Email,
        Password = httpRequest.Data.Password
      };

      user.Password = _passwordControllerAdapter.GeneratePassword(user.Password);

      var confirmationCode = new ConfirmationCode { UserId = user.Id };

      // create transaction
      await Task.WhenAll(
        _userRepository.Create(user),
        _confirmationCodeRepository.Create(confirmationCode)
      );

      _mailService.SendCodeConfirmation(user, confirmationCode.Code.ToString());

      return new HttpResponsePacket()
      {
        StatusCode = 200,
        Data = "Success!"
      };
    }
  }
}
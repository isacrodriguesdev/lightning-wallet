
using Protocol;
using Model;
using Helper;

using System.Text.Json;
using MongoDB.Bson;
using System.Net.Mail;

namespace Controller
{
  public class LoginUserController
  {
    private readonly IUserRepository _userRepository;
    private readonly IPasswordControllerAdapter _passwordControllerAdapter;
    private readonly ITokenControllerAdapter _tokenController;

    public LoginUserController(
      IUserRepository userRepository,
      IPasswordControllerAdapter passwordControllerAdapter,
      ITokenControllerAdapter tokenController
      )
    {
      _userRepository = userRepository;
      _passwordControllerAdapter = passwordControllerAdapter;
      _tokenController = tokenController;
    }

    public async Task<HttpResponsePacket> Execute(HttpRequestPacket<User> httpRequest)
    {

      User userRepo = await _userRepository.GetByEmail(httpRequest.Data.Email);

      if (userRepo == null || _passwordControllerAdapter.ComparePassword(userRepo.Password, httpRequest.Data.Password) == false)
      {
        return new HttpResponsePacket() { StatusCode = 401, Data = "Email or password incorrect" };
      }

      return new HttpResponsePacket()
      {
        StatusCode = 200,
        Data = new {
          Token = _tokenController.GenerateToken(userRepo) 
        }
      };
    }
  }
}
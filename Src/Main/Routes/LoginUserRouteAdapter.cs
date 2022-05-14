using Protocol;
using MongoDB;
using Model;

public static class LoginUserRouteAdapter
{

  public static async Task<Object> Router(User user, HttpContext httpContext)
  {

    var loginUserController = Factory.LoginUserFactory.Creator();

    var httpRequest = new HttpRequestPacket<User>() { Data = user };
    var httpResponse = await loginUserController.Execute(httpRequest);

    httpContext.Response.StatusCode = httpResponse.StatusCode;

    return httpResponse.Data;
  }
}
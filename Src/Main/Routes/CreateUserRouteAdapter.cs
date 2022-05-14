using Microsoft.EntityFrameworkCore;
using Protocol;
using Model;

public static class CreateUserRouteAdapter
{

  public static async Task<Object> Router(UserRegisterHttpRequest user, HttpContext httpContext)
  {
    var createUserController = Factory.CreateUserFactory.Creator();

    var httpRequest = new HttpRequestPacket<UserRegisterHttpRequest>() { Data = user };
    var httpResponse = await createUserController.Execute(httpRequest);

    httpContext.Response.StatusCode = httpResponse.StatusCode;

    return httpResponse.Data;
  }
}
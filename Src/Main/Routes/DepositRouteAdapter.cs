using System.Security.Claims;

using Protocol;

public class DepositRouteAdapter {

  public static async Task<Object> Router(HttpContext httpContext, ClaimsPrincipal user)
  {
    string userId = user.Claims.ToList()[0].Value;
    var depositController = Factory.DepositFactory.Creator();

    var httpRequest = new HttpRequestNoPacket(userId);
    var httpResponse = await depositController.Execute(httpRequest);

    httpContext.Response.StatusCode = httpResponse.StatusCode;

    Console.WriteLine(httpResponse.Data);

    return httpResponse.Data;
  }
}
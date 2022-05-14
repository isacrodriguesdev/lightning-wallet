using System.Security.Claims;
using Protocol;
using Model;

public static class GetWalletInfoRouteAdapter
{
  public static async Task<Object> Router(HttpContext httpContext, ClaimsPrincipal user)
  {
    string userId = user.Claims.ToList()[0].Value;
    var getWalletInfoController = GetWalletInfoFactory.Creator();

    var httpRequest = new HttpRequestNoPacket(userId);
    var httpResponse = await getWalletInfoController.Execute(httpRequest);

    httpContext.Response.StatusCode = httpResponse.StatusCode;

    return httpResponse.Data;
  }
}
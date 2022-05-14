using Protocol;
using MongoDB;
using Model;

public static class ConfirmationCodeRouteAdapter
{
  public static async Task<Object> Router(ConfirmationCodeHttpRequest confirmationCode, HttpContext httpContext)
  {
    var confirmationEmailController = ConfirmationEmailFactory.Creator();

    var httpRequest = new HttpRequestPacket<ConfirmationCodeHttpRequest>() { Data = confirmationCode };
    var httpResponse = await confirmationEmailController.Execute(httpRequest);

    httpContext.Response.StatusCode = httpResponse.StatusCode;

    return httpResponse.Data;
  }
}
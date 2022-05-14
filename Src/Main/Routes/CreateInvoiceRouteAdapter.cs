using System.Security.Claims;
using Protocol;

public static class CreateInvoiceRouteAdapter {
  public static async Task<Object> Router(CreateInvoiceHttpRequest data, HttpContext httpContext, ClaimsPrincipal user)
  {
    string userId = user.Claims.ToList()[0].Value;
    var invoiceFactory = Factory.CreateInvoiceFactory.Creator();

    var httpRequest = new HttpRequestPacket<CreateInvoiceHttpRequest>(userId) { Data = data };
    var httpResponse = await invoiceFactory.Execute(httpRequest);

    httpContext.Response.StatusCode = httpResponse.StatusCode;

    return httpResponse.Data;
  }
}
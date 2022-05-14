using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Protocol;

public static class SendPaymentRouteAdapter
{

  public static async Task<Object> Router(SendPaymentHttpRequest data, HttpContext httpContext, ClaimsPrincipal user)
  {
    string userId = user.Claims.ToList()[0].Value;
    var sendPaymentController = Factory.SendPaymentFactory.Creator();

    var httpRequest = new HttpRequestPacket<SendPaymentHttpRequest>(userId) { Data = data };
    var httpResponse = await sendPaymentController.Execute(httpRequest);

    httpContext.Response.StatusCode = httpResponse.StatusCode;

    return httpResponse.Data;
  }
}
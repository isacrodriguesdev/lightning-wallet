using System.Text.Json;
using Lnrpc;
using Factory;

namespace Adapter
{
  public class PaymentControllerAdapter : IPaymentControllerAdapter
  {
    private Lnrpc.Lightning.LightningClient _connection;

    public PaymentControllerAdapter()
    {
      var lnrpcConnection = LnrpcConnection.GetInstance();
      _connection = lnrpcConnection.GetConnection();
    }

    public async Task SendPaymentRequest(string userId, SendRequest sendRequest)
    {
      await HandlePaymentFactory.Creator(userId, sendRequest);
    }

    public async Task ListPayments()
    {
      var result = await _connection.ListPaymentsAsync(new ListPaymentsRequest());

      for (int i = 0; i < result.Payments.Count; i++)
      {
        Console.WriteLine(JsonSerializer.Serialize(result.Payments[i]));
      }
    }
  }
}
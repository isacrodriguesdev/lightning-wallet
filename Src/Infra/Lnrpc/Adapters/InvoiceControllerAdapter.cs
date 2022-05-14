using Lnrpc;

using System.Text.Json;

namespace Adapter
{
  public class InvoiceControllerAdapter : IInvoiceControllerAdapter
  {
    private Lnrpc.Lightning.LightningClient _connection;

    public InvoiceControllerAdapter()
    {
      var lnrpcConnection = LnrpcConnection.GetInstance();
      _connection = lnrpcConnection.GetConnection();
    }

    public async Task<string> AddInvoice(long amount, long expiry)
    {

      var result = await _connection.AddInvoiceAsync(
        new Invoice()
        {
          Value = amount,
          Expiry = expiry
        }
      );

      return result.PaymentRequest;
    }

    public Task<Invoice[]> ListInvoices()
    {
      throw new NotImplementedException();
    }
  }
}

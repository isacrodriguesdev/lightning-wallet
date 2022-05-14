using Lnrpc;


namespace Adapter
{
  public class DecodeControllerAdapter : IDecodeControllerAdapter
  {
    private Lnrpc.Lightning.LightningClient _connection;

    public DecodeControllerAdapter()
    {
      var lnrpcConnection = LnrpcConnection.GetInstance();
      _connection = lnrpcConnection.GetConnection();
    }

    public async Task<PayReq> DecodePaymentRequest(string paymentRequest)
    {
      try
      {
        return await _connection.DecodePayReqAsync(new PayReqString() { PayReq = paymentRequest });
      }
      catch (System.Exception error)
      {
        Console.WriteLine(error.Message);
        return null;
      }
    }
  }
}

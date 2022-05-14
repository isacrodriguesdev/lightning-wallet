using Lnrpc;

namespace Adapter
{
  public class WalletBitcoinControllerAdapter : IWalletBitcoinControllerAdapter
  {
    private Lightning.LightningClient _connection;

    public WalletBitcoinControllerAdapter()
    {
      var lnrpcConnection = LnrpcConnection.GetInstance();
      _connection = lnrpcConnection.GetConnection();
    }
    
    public async Task<string> NewAddress()
    {
      var result = await _connection.NewAddressAsync(new NewAddressRequest());
      return result.Address;
    }
  }
}
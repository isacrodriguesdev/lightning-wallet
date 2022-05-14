using Model;
using Protocol;
using Helper;

namespace Controller
{
  public class DepositController : IHttpController
  {
    private readonly IDepositAddressRepository _depositAddressRepository;
    private readonly IWalletBitcoinControllerAdapter _walletBitcoinControllerAdapter;

    public DepositController(IDepositAddressRepository depositAddressRepository, IWalletBitcoinControllerAdapter walletBitcoinControllerAdapter)
    {
      _depositAddressRepository = depositAddressRepository;
      _walletBitcoinControllerAdapter = walletBitcoinControllerAdapter;
    }

    public async Task<HttpResponsePacket> Execute(HttpRequestNoPacket httpRequest)
    {
      string address = await _walletBitcoinControllerAdapter.NewAddress();

      await _depositAddressRepository.Create(
        new DepositAddress
        {
          UserId = httpRequest.UserId,
          Address = address,
          Currency = TransactionCurrency.BTC,
          Network = TransactionNetwork.BITCOIN,
        }
      );

      return new HttpResponsePacket { Data = new { Address = address }, StatusCode = 200 };
    }
  }
}

namespace Factory
{
  public class DepositFactory
  {
    public static Controller.DepositController Creator()
    {
      var depositAddressRepository = new MongoDB.DepositAddressRepositoryAdapter();
      var walletBitcoinController = new Adapter.WalletBitcoinControllerAdapter();

      return new Controller.DepositController(depositAddressRepository, walletBitcoinController);
    }
  }
}
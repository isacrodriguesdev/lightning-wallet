using Controller;
using Adapter;
using MongoDB;

public class GetWalletInfoFactory
{
  public static GetWalletInfoController Creator()
  {
    var decodeControllerAdapter = new DecodeControllerAdapter();
    var transactionRepository = new TransactionRepositoryAdapter();
    var calculateUserBalanceController = new CalculateUserBalanceController(transactionRepository);

    return new GetWalletInfoController(
      transactionRepository,
      decodeControllerAdapter,
      calculateUserBalanceController
    );
  }
}

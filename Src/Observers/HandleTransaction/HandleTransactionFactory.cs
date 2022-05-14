using Observer;
using MongoDB;
using Helper;
using Adapter;
using Lnrpc;

namespace Factory
{
  public class HandleTransactionFactory {
    
    public static HandleTransactionObserver Creator() {

      var depositAddressRepository = new DepositAddressRepositoryAdapter();
      var transactionRepository = new TransactionRepositoryAdapter();
      var computeHash = new ComputeHash();

      var handleTransactionObserver = new HandleTransactionObserver(transactionRepository, depositAddressRepository, computeHash);
      var decodeControllerAdapter = new DecodeControllerAdapter();

      var transactionSubscribe = new BitcoinSubscribeControllerAdapter();
      transactionSubscribe.Attach(handleTransactionObserver);

      return handleTransactionObserver;
    }
  }
}
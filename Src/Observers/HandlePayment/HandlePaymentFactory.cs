using Observer;
using MongoDB;
using Helper;
using Adapter;
using Lnrpc;

namespace Factory
{
  public class HandlePaymentFactory {
    
    public static async Task<HandlePaymentObserver> Creator(string userId, SendRequest sendRequest) {

      var transactionRepository = new TransactionRepositoryAdapter();
      var computeHash = new ComputeHash();

      var handlePaymentObserver = new HandlePaymentObserver(transactionRepository, computeHash);
      var decodeControllerAdapter = new DecodeControllerAdapter();

      var paymentSubscribe = new PaymentSubscribeAdapter(decodeControllerAdapter);
      paymentSubscribe.Attach(handlePaymentObserver);
      
      await paymentSubscribe.SendPaymentRequest(userId, sendRequest);

      return handlePaymentObserver;
    }
  }
}
using Controller;
using Adapter;
using MongoDB;
using Helper;

namespace Factory
{
  public class SendPaymentFactory
  {
    public static SendPaymentController Creator()
    {
      var paymentControllerAdapter = new PaymentControllerAdapter();
      var decodeControllerAdapter = new DecodeControllerAdapter();
      var computeHashHelper = new ComputeHash();
      var transactionRepository = new TransactionRepositoryAdapter();
      var calculateUserBalanceController = new CalculateUserBalanceController(transactionRepository);
      
      return new SendPaymentController(
        paymentControllerAdapter,
        computeHashHelper,
        decodeControllerAdapter,
        transactionRepository,
        calculateUserBalanceController
      );
    }
  }
}
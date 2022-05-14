using Observer;
using MongoDB;
using Helper;
using Adapter;

namespace Factory
{
  public class HandleInvoiceFactory {
    
    public static HandleInvoiceObserver Creator() {

      var transactionRepository = new TransactionRepositoryAdapter();
      var decodeControllerAdapter = new DecodeControllerAdapter();
      var computeHash = new ComputeHash();

      var handleInvoiceObserver = new HandleInvoiceObserver(transactionRepository, computeHash);

      var subscribeInvoice = new InvoiceSubscribeAdapter();
      subscribeInvoice.Attach(handleInvoiceObserver);

      return handleInvoiceObserver;
    }
  }
}
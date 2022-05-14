using Adapter;
using Helper;

namespace Factory
{
  public class CreateInvoiceFactory
  {
    public static Controller.CreateInvoiceController Creator()
    {
      var invoiceControllerAdapter = new InvoiceControllerAdapter();
      var transactionRepositoryAdapter = new MongoDB.TransactionRepositoryAdapter();
      var decodeControllerAdapter = new DecodeControllerAdapter();
      var computeHashHelper = new ComputeHash();

      return new Controller.CreateInvoiceController(
        invoiceControllerAdapter,
        transactionRepositoryAdapter,
        computeHashHelper,
        decodeControllerAdapter
      );
    }
  }
}
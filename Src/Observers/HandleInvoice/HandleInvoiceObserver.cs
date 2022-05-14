using System.Text.Json;
using Adapter;
using Lnrpc;
using Helper;

namespace Observer
{
  public class HandleInvoiceObserver : IObserver
  {
    private readonly ITransactionRepository _transactionRepository;
    private readonly IComputeHashControllerAdapter _computeHash;

    public HandleInvoiceObserver(ITransactionRepository transactionRepository, IComputeHashControllerAdapter computeHash)
    {
      _transactionRepository = transactionRepository;
      _computeHash = computeHash;
    }

    public async Task Execute(ISubject _subject)
    {
      var subscribe = _subject as InvoiceSubscribeAdapter;
      var current = subscribe.Current;

      var trasanction = await _transactionRepository.GetUserOneTransactionByStatus(
        current.PaymentRequest, Model.TransactionStatus.OPEN
      );

      if(trasanction == null) return;

      int currentStatus = current.State == Invoice.Types.InvoiceState.Settled ? 
        Model.TransactionStatus.SUCCESS : Model.TransactionStatus.ERROR;

      trasanction.Hash = null;
      trasanction.Status = currentStatus;
      trasanction.Hash = _computeHash.GenerateSHA256(JsonSerializer.Serialize(trasanction));

      await _transactionRepository.UpdateUserTransaction(
        current.PaymentRequest,
        new Model.Transaction {
          Status = currentStatus,
          Hash = trasanction.Hash,
        }
      );
    }
  }
}
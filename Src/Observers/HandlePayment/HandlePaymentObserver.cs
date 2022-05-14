using System.Text.Json;
using Adapter;
using Lnrpc;
using Helper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Observer
{
  public class HandlePaymentObserver : IObserverPayment
  {
    private readonly ITransactionRepository _transactionRepository;
    private readonly IComputeHashControllerAdapter _computeHash;

    public HandlePaymentObserver(ITransactionRepository transactionRepository, IComputeHashControllerAdapter computeHash)
    {
      _transactionRepository = transactionRepository;
      _computeHash = computeHash;
    }

    public async Task Execute(ISubjectPayment _subject)
    {
      var subscribe = _subject as PaymentSubscribeAdapter;
      var currentPayment = subscribe.CurrentPayment;
      var currentUserId = subscribe.CurrentUserId;

      var transaction = new Model.Transaction
      {
        UserId = currentUserId,
        Address = currentPayment.PaymentRequest,
        Amount = currentPayment.NumSatoshis,
        Destination = currentPayment.Destination,
        Status = Model.TransactionStatus.SUCCESS,
        Operation = Model.TransactionOperation.EXTERNAL_TRANSFER,
        Type = Model.TransactionType.OUTPUT,
        Currency = Model.TransactionCurrency.BTC,
        Network = Model.TransactionNetwork.LIGHTNING,
        CreatedAt = currentPayment.Timestamp,
      };

      transaction.Hash = _computeHash.GenerateSHA256(JsonSerializer.Serialize(transaction));

      await Task.WhenAll(
        _transactionRepository.Create(transaction)
      );
    }
  }
}
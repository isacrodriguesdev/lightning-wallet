using System.Text.Json;
using Adapter;
using Lnrpc;
using Helper;

namespace Observer
{
  public class HandleTransactionObserver : IObserver
  {
    private readonly ITransactionRepository _transactionRepository;
    private readonly IDepositAddressRepository _depositAddressRepository;
    private readonly IComputeHashControllerAdapter _computeHash;

    public HandleTransactionObserver(ITransactionRepository transactionRepository, IDepositAddressRepository depositAddressRepository, IComputeHashControllerAdapter computeHash)
    {
      _transactionRepository = transactionRepository;
      _computeHash = computeHash;
      _depositAddressRepository = depositAddressRepository;
    }

    public async Task Execute(ISubject _subject)
    {
      var subscribe = _subject as BitcoinSubscribeControllerAdapter;
      var transactionCurrent = subscribe.Current;

      string address = transactionCurrent.OutputDetails[transactionCurrent.OutputDetails.Count - 1].Address;
      var deposit = await _depositAddressRepository.FindByAddress(address);

      if (transactionCurrent.NumConfirmations >= 1)
      {
        var transaction = new Model.Transaction
        {
          UserId = deposit.UserId,
          Address = address,
          Amount = transactionCurrent.Amount,
          Status = Model.TransactionStatus.SUCCESS,
          Operation = Model.TransactionOperation.INTERNAL_TRANSFER,
          Type = Model.TransactionType.INPUT,
          Currency = Model.TransactionCurrency.BTC,
          Network = Model.TransactionNetwork.BITCOIN,
          CreatedAt = transactionCurrent.TimeStamp,
        };

        transaction.Hash = _computeHash.GenerateSHA256(JsonSerializer.Serialize(transaction));

        await Task.WhenAll(
          _transactionRepository.Create(transaction)
        );
      }
    }
  }
}
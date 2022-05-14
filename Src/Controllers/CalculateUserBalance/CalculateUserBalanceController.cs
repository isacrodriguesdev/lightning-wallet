
namespace Controller
{
  public class CalculateUserBalanceController : ICalculateUserBalanceController
  {
    private readonly ITransactionRepository _transactionRepository;

    public CalculateUserBalanceController(ITransactionRepository transactionRepository)
    {
      _transactionRepository = transactionRepository;
    }

    public async Task<long> Execute(string userId)
    {
      long balance = 0;
      var transactions = await _transactionRepository.GetUserTransactionsValid(userId);

      foreach (var transaction in transactions)
      {
        if (transaction.Type == Model.TransactionType.INPUT)
        {
          balance += transaction.Amount;
        }
        else
        {
          balance -= transaction.Amount;
        }
      }

      return balance;
    }
  }
}
using Model;

public interface ITransactionRepository
{
  Task Create(Transaction transaction);
  Task UpdateUserTransaction(string transactionId, Transaction transaction);
  Task<Transaction> GetUserOneTransactionByStatus(string transactionId, int status);
  Task<Transaction> GetUserOneTransactionByAddress(string address);
  Task<List<Transaction>> GetUserTransactions(string userId);
  Task<List<Transaction>> GetUserTransactionsByStatus(string userId, int status);
  Task<List<Transaction>> GetUserTransactionsValid(string userId);
}

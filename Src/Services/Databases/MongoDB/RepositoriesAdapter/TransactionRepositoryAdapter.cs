using MongoDB.Driver;
using Model;

namespace MongoDB
{
  public class TransactionRepositoryAdapter : ITransactionRepository
  {
    private readonly IMongoDatabase _context;
    private readonly IMongoCollection<Transaction> _collection;

    public TransactionRepositoryAdapter()
    {
      var mongoConnection = MongoConnection.GetInstance();
      _context = mongoConnection.GetConnection();
      _collection = _context.GetCollection<Transaction>("Transactions");

      List<IndexKeysDefinition<Transaction>> validations = new List<IndexKeysDefinition<Transaction>>() {
        Builders<Transaction>.IndexKeys.Ascending(x => x.Hash),
        Builders<Transaction>.IndexKeys.Ascending(x => x.Address)
      };

      foreach (var validation in validations)
      {
        _collection.Indexes.CreateOne(
          new CreateIndexModel<Transaction>(validation, new CreateIndexOptions { Unique = true })
        );
      }
    }

    public async Task Create(Transaction transaction)
    {
      await _collection.InsertOneAsync(transaction);
    }

    public async Task<Transaction> GetUserOneTransactionByStatus(string address, int status)
    {
      var doc = await _collection.FindAsync<Transaction>(
          Builders<Transaction>.Filter.And(
            Builders<Transaction>.Filter.Eq(x => x.Address, address),
            Builders<Transaction>.Filter.Eq(x => x.Status, status)
        )
      );
      return await doc.FirstOrDefaultAsync();
    }

    public async Task<Transaction> GetUserOneTransactionByAddress(string address)
    {
      var doc = await _collection.FindAsync<Transaction>(
          Builders<Transaction>.Filter.Eq(x => x.Address, address)
      );
      return await doc.FirstOrDefaultAsync();
    }

    public async Task<List<Transaction>> GetUserTransactions(string userId)
    {
      var doc = await _collection.FindAsync<Transaction>(
         Builders<Transaction>.Filter.And(
           Builders<Transaction>.Filter.Eq(x => x.UserId, userId)
       )
     );
      return await doc.ToListAsync();
    }

    public async Task<List<Transaction>> GetUserTransactionsByStatus(string userId, int status)
    {
      var doc = await _collection.FindAsync<Transaction>(
          Builders<Transaction>.Filter.And(
            Builders<Transaction>.Filter.Eq(x => x.UserId, userId),
            Builders<Transaction>.Filter.Eq(x => x.Status, status)
        )
      );
      return await doc.ToListAsync();
    }

    public async Task<List<Transaction>> GetUserTransactionsValid(string userId)
    {
      var doc = await _collection.FindAsync<Transaction>(
          Builders<Transaction>.Filter.And(
            Builders<Transaction>.Filter.Eq(x => x.UserId, userId),
            Builders<Transaction>.Filter.In(x => x.Status, new[] { Model.TransactionStatus.SUCCESS, Model.TransactionStatus.SUCCESS })
        )
      );
      return await doc.ToListAsync();
    }

    public async Task UpdateUserTransaction(string address, Transaction transaction)
    {
      var filter = Builders<Transaction>.Filter.Eq(x => x.Address, address);

      var update = Builders<Transaction>.Update
        .Set("Hash", transaction.Hash)
        .Set("Status", transaction.Status);

      await _collection.UpdateOneAsync(filter, update);
    }
  }
}
using Model;
using MongoDB.Driver;

namespace MongoDB
{
  public class DepositAddressRepositoryAdapter : IDepositAddressRepository
  {

    private readonly IMongoDatabase _context;
    private readonly IMongoCollection<DepositAddress> _collection;

    public DepositAddressRepositoryAdapter() {
      var mongoConnection = MongoConnection.GetInstance();
      _context = mongoConnection.GetConnection();

      _collection = _context.GetCollection<DepositAddress>("DepositAddresses");
    }

    public async Task Create(DepositAddress depositAddress)
    {
      await _collection.InsertOneAsync(depositAddress);
    }

    public async Task<DepositAddress> FindByAddress(string address)
    {
      try
      {
        var doc = await _collection.FindAsync<DepositAddress>(
           Builders<DepositAddress>.Filter.Eq("Address", address)
         );
        var depositAddress = doc.FirstOrDefault();

        return depositAddress;
      }
      catch (System.Exception)
      {
        return null;
      }
    }
  }
}
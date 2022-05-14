using Factory;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDB
{
  public class MongoConnection
  {

    private static MongoConnection _context;
    private static MongoClient _client;
    private IMongoDatabase _mongoDatabase;

    public static MongoConnection GetInstance()
    {
      if (_context == null)
      {
        _context = new MongoConnection();
      }
      return _context;
    }

    public IMongoDatabase GetConnection()
    {
      return _mongoDatabase;
    }

    public MongoClient GetClient()
    {
      return _client;
    }

    private MongoConnection()
    {
      _mongoDatabase = CreateConnection();
    }

    private IMongoDatabase CreateConnection()
    {
      var getConnectionController = GetConnectionStringFactory.Creator();
      var client = new MongoClient(getConnectionController.Execute("MongoDB"));
      _client = client;

      return client.GetDatabase("MyDatabase");
    }
  }
}
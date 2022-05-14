
using Model;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDB
{

  public class UserRepositoryAdapter : IUserRepository
  {

    private readonly Driver.IMongoDatabase _context;
    private readonly IMongoCollection<User> _collection;

    public UserRepositoryAdapter()
    {
      _context = MongoConnection.GetInstance().GetConnection();
      _collection = _context.GetCollection<User>("Users");

      List<IndexKeysDefinition<User>> validations = new List<IndexKeysDefinition<User>>() {
        Builders<User>.IndexKeys.Ascending(x => x.Email),
        Builders<User>.IndexKeys.Ascending(x => x.Username)
      };

      foreach (var validation in validations)
      {
        _collection.Indexes.CreateOne(
          new CreateIndexModel<User>(validation, new CreateIndexOptions { Unique = true })
        );
      }
    }

    public async Task Create(User user)
    {
      await Task.FromResult(_collection.InsertOneAsync(user));
    }

    public async Task<User> GetByEmail(string email)
    {
      var userFilter = Builders<User>.Filter.Eq(x => x.Email, email);
      var confirmedFilter = Builders<User>.Filter.Eq(x => x.ConfirmedAccount, true);

      var doc = await _collection.FindAsync<User>(
        Builders<User>.Filter.And(userFilter, confirmedFilter)
      );

      return doc.FirstOrDefault();
    }

    public async Task<User> GetById(string id)
    {
      var userFilter = Builders<User>.Filter.Eq(x => x.Id, id);
      var confirmedFilter = Builders<User>.Filter.Eq(x => x.ConfirmedAccount, true);

      var doc = await _collection.FindAsync<User>(
        Builders<User>.Filter.And(userFilter, confirmedFilter)
      );

      return doc.FirstOrDefault();
    }

    public async Task<User> GetByUsername(string username)
    {
      var userFilter = Builders<User>.Filter.Eq(x => x.Username, username);
      var confirmedFilter = Builders<User>.Filter.Eq(x => x.ConfirmedAccount, true);

      var doc = await _collection.FindAsync<User>(
        Builders<User>.Filter.And(userFilter, confirmedFilter)
      );

      return doc.FirstOrDefault();
    }

    public async Task<User> GetUnverifiedUserById(string id)
    {
      var userFilter = Builders<User>.Filter.Eq(x => x.Id, id);
      var confirmedFilter = Builders<User>.Filter.Eq(x => x.ConfirmedAccount, false);

      var doc = await _collection.FindAsync<User>(
        Builders<User>.Filter.And(userFilter, confirmedFilter)
      );

      return doc.FirstOrDefault();
    }

    public async Task UpdateConfirmedAccount(string userId, bool confirmedCode = true)
    {
      await _collection.UpdateOneAsync(
        Builders<User>.Filter.Eq(x => x.Id, userId),
        Builders<User>.Update.Set("ConfirmedAccount", confirmedCode)
      );
    }
  }
}
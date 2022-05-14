using Model;
using MongoDB.Driver;

namespace MongoDB
{
  public class ConfirmationCodeRepositoryAdapter : IConfirmationCodeRepository
  {
    private readonly IMongoDatabase _context;
    private readonly IMongoCollection<ConfirmationCode> _collection;

    public ConfirmationCodeRepositoryAdapter()
    {
      var mongoConnection = MongoConnection.GetInstance();
      _context = mongoConnection.GetConnection();

      _collection = _context.GetCollection<ConfirmationCode>("ConfirmationCodes");
    }

    public async Task Create(ConfirmationCode confirmationCode)
    {
      try
      {
        await _collection.InsertOneAsync(confirmationCode);
      }
      catch (System.Exception)
      {
        return;
      }
    }

    public async Task<ConfirmationCode> FindCode(int code)
    {
      try
      {
        var doc = await _collection.FindAsync<ConfirmationCode>(
           Builders<ConfirmationCode>.Filter.Eq("Code", code)
         );
        var confirmationCode = doc.FirstOrDefault();

        return confirmationCode;

      }
      catch (System.Exception)
      {
        return null;
      }
    }

    public async Task<ConfirmationCode> FindCode(string id)
    {
      try
      {
        var doc = await _collection.FindAsync<ConfirmationCode>(
           Builders<ConfirmationCode>.Filter.Eq(x => x.Id, id)
         );
        var confirmationCode = doc.FirstOrDefault();

        return confirmationCode;

      }
      catch (System.Exception)
      {
        return null;
      }
    }
  }
}
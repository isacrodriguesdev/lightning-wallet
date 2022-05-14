using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
  public static class TransactionOperation
  {
    public const int CREATED_INVOICE = 10;

    public const int EXTERNAL_TRANSFER = 20;
    public const int INTERNAL_TRANSFER = 21;
    public const int TRANSFER_TO_CONTACT = 22;

    public const int PHONE_RECHARGE = 100;
  }

  public static class TransactionStatus
  {
    public const int OPEN = 1;
    public const int SUCCESS = 2;
    public const int ERROR = 30;
    public const int NO_ROUTE = 31;
  }

  public static class TransactionType
  {
    public const int INPUT = 1;
    public const int OUTPUT = 2;
  }

  public static class TransactionNetwork
  {
    public const int LIGHTNING = 1;
    public const int BITCOIN = 2;
  }

  public static class TransactionCurrency
  {
    public const int BTC = 1;
  }

  public class Transaction
  {
    private long _currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    private long _createdAt;
    private long _expiry;

    [BsonId]
    public string Id { get; set; } = GenerateGuid.Handle();
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Hash { get; set; }
    public string UserId { get; set; }
    public string Address { get; set; }
    public long Amount { get; set; }
    public string Description { get; set; }
    public string Destination { get; set; }
    public int Type { get; set; }
    public int Currency { get; set; }
    public int Operation { get; set; }
    public int Status { get; set; }
    public int Network { get; set; }
    public long Expiry
    {
      get { return _expiry; }
      set
      {
        _expiry = value < _currentTimestamp ? value * 1000 : value;
      }
    }
    public long CreatedAt
    {
      get { return _createdAt; }
      set
      {
        _createdAt = value < _currentTimestamp ? value * 1000 : value;
      }
    }

    public Transaction() { }

    public Transaction(Transaction transaction)
    {
      Hash = transaction.Hash;
      Id = transaction.Id;
      UserId = transaction.UserId;
      Address = transaction.Address;
      Status = transaction.Status;
      Amount = transaction.Amount;
      Type = transaction.Type;
      Operation = transaction.Operation;
      Destination = transaction.Destination;
      _expiry = transaction.Expiry;
      _createdAt = transaction.CreatedAt;
    }
  }
}
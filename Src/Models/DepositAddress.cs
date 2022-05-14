using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

public class DepositAddress {
  [BsonId]
  public string Id { get; set; } = GenerateGuid.Handle();
  public string Address { get; set; }
  public string UserId { get; set; }
  public int Currency { get; set; }
  public int Network { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
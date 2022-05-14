using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
  public class ConfirmationCode
  {
    [BsonId]
    public string Id { get; set; } = GenerateGuid.Handle();
    public string UserId { get; set; }
    public int Code { get; set; } = RandomDigits.Handle(4);
    public string Method { get; set; } = "EMAIL"; // EMAIL, PHONE 
    public DateTime Expiry { get; set; } = DateTime.UtcNow.AddMinutes(60);
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  }
}
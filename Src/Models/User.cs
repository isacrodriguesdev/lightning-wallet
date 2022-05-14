using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
  public class User
  {

    private string _username;
    private string _email;

    [BsonId]
    public string Id { get; set; } = GenerateGuid.Handle();
    public string Name { get; set; }
    public string Photo { get; set; }
    public string Username
    {
      get => _username; set
      {
        if (value != null)
        {
          _username = value
            .ToLower()
            .Trim();
        }
      }
    }
    public string Email
    {
      get => _email; set
      {
        _email = value
          .ToLower()
          .Trim();
      }
    }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public bool ConfirmedAccount { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User() { }

    public User(User user)
    {
      Id = user.Id;
      Name = user.Name;
      Username = user.Username;
      Email = user.Email;
      Password = user.Password;
      ConfirmedAccount = user.ConfirmedAccount;
      CreatedAt = user.CreatedAt;
    }
  }
}


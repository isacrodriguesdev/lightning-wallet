using Model;

public interface IUserRepository
{
  Task Create(User user);
  Task<User> GetByEmail(string email);
  Task<User> GetById(string id);
  Task<User> GetUnverifiedUserById(string id);
  Task<User> GetByUsername(string username);
  Task UpdateConfirmedAccount(string userId, bool confirmedCode = true);
}


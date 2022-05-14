using Model;

public interface IConfirmationCodeRepository
{
  Task Create(ConfirmationCode confirmationCode);
  Task<ConfirmationCode> FindCode(int code);
  Task<ConfirmationCode> FindCode(string id);
}


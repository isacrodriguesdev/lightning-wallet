using Model;
public interface IMailService {
  void SendCodeConfirmation(User user, string code);
}
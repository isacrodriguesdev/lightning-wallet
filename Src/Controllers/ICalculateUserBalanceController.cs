
public interface ICalculateUserBalanceController {
  Task<long> Execute(string userId);
}
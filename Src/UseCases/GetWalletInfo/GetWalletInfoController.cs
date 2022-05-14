using Model;
using Protocol;

namespace Controller
{
  public class GetWalletInfoController : IHttpController
  {
    private readonly IDecodeControllerAdapter _decodedControllerAdapter;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICalculateUserBalanceController _calculateUserBalanceController;

    public GetWalletInfoController(ITransactionRepository transactionRepository, IDecodeControllerAdapter decodeControllerAdapter, ICalculateUserBalanceController calculateUserBalanceController)
    {
      _decodedControllerAdapter = decodeControllerAdapter;
      _transactionRepository = transactionRepository;
      _calculateUserBalanceController = calculateUserBalanceController;
    }

    public async Task<HttpResponsePacket> Execute(HttpRequestNoPacket httpRequest)
    {
      var transactions = await _transactionRepository.GetUserTransactions(httpRequest.UserId);
      long balance = await _calculateUserBalanceController.Execute(httpRequest.UserId);

      return new HttpResponsePacket
      {
        Data = new
        {
          UserId = httpRequest.UserId,
          TotalSat = balance,
          TotalBtc = (decimal)balance / 100000000,
          Transactions = transactions
        },
        StatusCode = 200
      };
    }
  }
}
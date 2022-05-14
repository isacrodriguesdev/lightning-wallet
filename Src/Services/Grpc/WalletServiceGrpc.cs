using Grpc.Core;
using Factory;
using Model;
using Protocol;

public class WalletServiceGrpc : Wallet.WalletBase
{
  private readonly ILogger<WalletServiceGrpc> _logger;

  public WalletServiceGrpc(ILogger<WalletServiceGrpc> logger)
  {
    _logger = logger;
  }

  public async override Task<BalanceResponse> SyncWalletBalance(UserRequest request, ServerCallContext context)
  {
    // var getUserBalanceController = GetWalletTransactionsFactory.Creator();

    // var httpRequestPackat = new HttpRequestPacket<User>
    // {
    //   Data = new User()
    //   {
    //     Id = new MongoDB.Bson.ObjectId(request.Id)
    //   }
    // };
    // var httpResponsePacket = await getUserBalanceController.Execute(httpRequestPackat);
 
    // correct the check and put object type checked((long)httpResponsePacket.Data)
    return await Task.FromResult(new BalanceResponse() { Value = 10 });
  }
}
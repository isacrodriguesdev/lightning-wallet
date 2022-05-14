using System.Text.Json;
using Protocol;


using Helper;
using MongoDB.Bson;
using Lnrpc;

namespace Controller
{
  public class SendPaymentController : IHttpController<SendPaymentHttpRequest>
  {
    private readonly IPaymentControllerAdapter _paymentController;
    private readonly IDecodeControllerAdapter _decodedController;
    private readonly IComputeHashControllerAdapter _computeHash;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICalculateUserBalanceController _calculateUserBalanceController;

    // inject dependencies
    public SendPaymentController(IPaymentControllerAdapter paymentController, IComputeHashControllerAdapter computeHash, IDecodeControllerAdapter decodeController, ITransactionRepository transactionRepository, ICalculateUserBalanceController calculateUserBalanceController)
    {
      _paymentController = paymentController;
      _computeHash = computeHash;
      _decodedController = decodeController;
      _transactionRepository = transactionRepository;
      _calculateUserBalanceController = calculateUserBalanceController;
    }

    // logical code here
    public async Task<HttpResponsePacket> Execute(HttpRequestPacket<SendPaymentHttpRequest> httpRequest)
    {
      var sendPaymentValidator = new Validator.SendPaymentHttpRequestValidator();
      var validationResult = sendPaymentValidator.Validate(httpRequest.Data);

      if (validationResult.IsValid == false)
        return new HttpResponsePacket() { Data = new { Errors = validationResult.Errors }, StatusCode = 400 };

      var decodedPayment = await _decodedController.DecodePaymentRequest(httpRequest.Data.PaymentRequest);

      if (decodedPayment == null)
        return new HttpResponsePacket() { Data = "Invalid address", StatusCode = 400 };

      long balance = await _calculateUserBalanceController.Execute(httpRequest.UserId);

      if (decodedPayment.NumSatoshis > balance)
      {
        return new HttpResponsePacket()
        {
          Data = new
          {
            Error = "without balance",
            Balance = balance,
            Invoice = decodedPayment.NumSatoshis
          }
        };
      }

      var transaction = _transactionRepository.GetUserOneTransactionByAddress(httpRequest.Data.PaymentRequest);

      var sendRequest = new SendRequest
      {
        PaymentRequest = httpRequest.Data.PaymentRequest,
        Amt = httpRequest.Data.Amount
      };

      if (transaction == null)
      {
        await _paymentController.SendPaymentRequest(httpRequest.UserId, sendRequest);
      }
      else
      {
        sendRequest.AllowSelfPayment = true;
        await _paymentController.SendPaymentRequest(httpRequest.UserId, sendRequest);
      }

      return new HttpResponsePacket()
      {
        Data = "Payment completed with success!",
        StatusCode = 200
      };
    }
  }
}
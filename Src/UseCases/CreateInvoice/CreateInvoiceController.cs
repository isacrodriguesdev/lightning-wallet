

using Protocol;

using MongoDB.Bson;
using Helper;
using Lnrpc;
using System.Text.Json;

namespace Controller
{
  public class CreateInvoiceController : IHttpController<CreateInvoiceHttpRequest>
  {

    private readonly IDecodeControllerAdapter _decodedControllerAdapter;
    private readonly IInvoiceControllerAdapter _invoiceControllerAdapter;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IComputeHashControllerAdapter _computeHash;

    public CreateInvoiceController(
      IInvoiceControllerAdapter invoiceControllerAdapter,
      ITransactionRepository transactionRepository,
      IComputeHashControllerAdapter computeHash,
      IDecodeControllerAdapter decodeControllerAdapter
      )
    {
      _transactionRepository = transactionRepository;
      _invoiceControllerAdapter = invoiceControllerAdapter;
      _computeHash = computeHash;
      _decodedControllerAdapter = decodeControllerAdapter;
    }

    public async Task<HttpResponsePacket> Execute(HttpRequestPacket<CreateInvoiceHttpRequest> httpRequest)
    {
      var createInvoiceValidator = new Validator.CreateInvoiceHttpRequestValidator();
      var validationResult = createInvoiceValidator.Validate(httpRequest.Data);

      if (validationResult.IsValid == false)
      {
        return new HttpResponsePacket()
        {
          Data = new { Errors = validationResult.Errors },
          StatusCode = 400
        };
      }

      try
      {
        string paymentRequest = await _invoiceControllerAdapter.AddInvoice(
          httpRequest.Data.Amount,
          httpRequest.Data.Expiry
        );
        var paymentRequestDecoded = await _decodedControllerAdapter.DecodePaymentRequest(paymentRequest);

        var transaction = new Model.Transaction
        {
          UserId = httpRequest.UserId,
          Address = paymentRequest,
          Amount = paymentRequestDecoded.NumSatoshis,
          Expiry = DateTimeOffset.UtcNow.AddSeconds(paymentRequestDecoded.Expiry).ToUnixTimeMilliseconds(),
          Status = Model.TransactionStatus.OPEN,
          Type = Model.TransactionType.INPUT,
          Currency = Model.TransactionCurrency.BTC,
          Operation = Model.TransactionOperation.CREATED_INVOICE,
          Network = Model.TransactionNetwork.LIGHTNING,
          CreatedAt = paymentRequestDecoded.Timestamp,
        };

        transaction.Hash = _computeHash.GenerateSHA256(JsonSerializer.Serialize(transaction));

        await _transactionRepository.Create(transaction);

        return new HttpResponsePacket()
        {
          StatusCode = 200,
          Data = paymentRequest
        };
      }
      catch (System.Exception)
      {
        return new HttpResponsePacket()
        {
          StatusCode = 500,
          Data = "Creation transaction failed"
        };
      }

    }
  }
}
using System.Text.Json;
using Lnrpc;

using Model;

namespace Adapter
{
  public class PaymentSubscribeAdapter : ISubjectPayment
  {
    private readonly Lnrpc.Lightning.LightningClient _connection;
    private readonly List<IObserverPayment> _observers = new List<IObserverPayment>();
    private readonly IDecodeControllerAdapter _decodedController;

    public PayRes CurrentPayment { get; set; }
    public string CurrentUserId { get; set; }
    public string Failure { get; set; }

    public PaymentSubscribeAdapter(IDecodeControllerAdapter decodedController)
    {
      var lnrpcConnection = LnrpcConnection.GetInstance();
      _connection = lnrpcConnection.GetConnection();
      _decodedController = decodedController;
    }

    public void Attach(IObserverPayment observer)
    {
      _observers.Add(observer);
    }

    public void Notify()
    {
      _observers.ForEach(observer =>
      {
        observer.Execute(this);
      });
    }

    public async Task SendPaymentRequest(string userId, SendRequest sendRequest)
    {
      CurrentUserId = userId;
      var paymentDecoded = await _decodedController.DecodePaymentRequest(sendRequest.PaymentRequest);

      using (var call = _connection.SendPayment())
      {
        var responseReaderTask = Task.Run(async () =>
        {
          while (await call.ResponseStream.MoveNext(new CancellationToken()))
          {
            CurrentPayment = new PayRes {
              PaymentRequest = sendRequest.PaymentRequest,
              Description = paymentDecoded.Description,
              NumSatoshis = paymentDecoded.NumSatoshis,
              Destination = paymentDecoded.Destination,
              Expiry = paymentDecoded.Expiry,
              Timestamp = paymentDecoded.Timestamp
            };
            Notify();
          }
        });

        foreach (SendRequest _sendRequest in _SendPaymentRequest(sendRequest))
        {
          try
          {
            await call.RequestStream.WriteAsync(_sendRequest);
          }
          catch (Grpc.Core.RpcException e)
          {
            Failure = e.Message;
          }
        }
        await call.RequestStream.CompleteAsync();
        await responseReaderTask;
      }
      
    }

    private IEnumerable<SendRequest> _SendPaymentRequest(SendRequest sendRequest)
    {
      yield return sendRequest;
    }
  }
}
using System.Text.Json;
using Lnrpc;

namespace Adapter
{
  public class InvoiceSubscribeAdapter : ISubject
  {

    private readonly Lightning.LightningClient _connection;
    private static readonly List<IObserver> _observers = new List<IObserver>();

    public Invoice Current { get; set; }

    public InvoiceSubscribeAdapter()
    {
      var lnrpcConnection = LnrpcConnection.GetInstance();
      _connection = lnrpcConnection.GetConnection();
      Task.Run(RunSubscribe);
    }

    public void Attach(IObserver observer)
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

    private async Task RunSubscribe()
    {
      using (var call = _connection.SubscribeInvoices(new InvoiceSubscription()))
      {
        while (await call.ResponseStream.MoveNext(new CancellationToken()))
        {
          Current = call.ResponseStream.Current;
          Notify();
        }
      }
    }
  }
}
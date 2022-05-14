using Lnrpc;
using System.Text.Json;

namespace Adapter
{
  public class BitcoinSubscribeControllerAdapter : ISubject
  {

    private Transaction _current;

    private readonly Lightning.LightningClient _connection;
    private static readonly List<IObserver> _observers = new List<IObserver>();

    public Transaction Current { get => _current; private set { _current = value; } }

    public BitcoinSubscribeControllerAdapter()
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
      using (var call = _connection.SubscribeTransactions(new GetTransactionsRequest() { StartHeight = 2197406 }))
      {
        while (await call.ResponseStream.MoveNext(new CancellationToken()))
        {
          _current = call.ResponseStream.Current;
          Notify();
        }
      }
    }
  }
}
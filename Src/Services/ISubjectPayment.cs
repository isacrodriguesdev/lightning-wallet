using Lnrpc;


  public interface ISubjectPayment {
    void Attach(IObserverPayment observer);
    void Notify();
    Task SendPaymentRequest(string userId, SendRequest sendRequest);
  }

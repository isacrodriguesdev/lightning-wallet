using Lnrpc;

public interface IPaymentControllerAdapter
{
  Task SendPaymentRequest(string userId, SendRequest sendRequest);
  Task ListPayments();
}

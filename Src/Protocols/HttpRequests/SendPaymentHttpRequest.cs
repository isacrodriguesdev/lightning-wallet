
namespace Protocol
{
  public class SendPaymentHttpRequest {
    public long Amount { get; set; } = 0;
    public string PaymentRequest { get; set; }
  }
}
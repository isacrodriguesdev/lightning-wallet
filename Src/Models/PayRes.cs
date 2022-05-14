using Lnrpc;

namespace Model
{
  public class PayRes
  {
    public string PaymentRequest { get; set; }
    public string Destination { get; set; }
    public long NumSatoshis { get; set; }
    public long Timestamp { get; set; }
    public long Expiry { get; set; }
    public string Description { get; set; }
  }
}

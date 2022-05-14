using Lnrpc;


public interface IDecodeControllerAdapter
{
  public Task<PayReq> DecodePaymentRequest(string paymentRequest);
}

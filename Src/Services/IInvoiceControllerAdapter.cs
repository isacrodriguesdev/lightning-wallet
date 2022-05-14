using Lnrpc;


public interface IInvoiceControllerAdapter
{
  public Task<string> AddInvoice(long amount, long expiry);
  public Task<Invoice[]> ListInvoices();
}

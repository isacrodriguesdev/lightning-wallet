using Factory;
using MongoDB;

namespace Nodo14
{
  public class Startup
  {
    static void Main(string[] args)
    {
      System.Environment.SetEnvironmentVariable("GRPC_SSL_CIPHER_SUITES", "HIGH+ECDSA");

      App.CreateApp(args);

      LnrpcConnection.GetInstance();
      MongoConnection.GetInstance();
      
      HandleInvoiceFactory.Creator();
      HandleTransactionFactory.Creator();

      App.Run();
    }
  }
}
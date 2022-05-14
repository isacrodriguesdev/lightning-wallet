using Lnrpc;
using Grpc.Core;
using Grpc.Net.Client;
using System.Security.Cryptography.X509Certificates;

public class LnrpcConnection
{
  private static LnrpcConnection _instance;
  private Lightning.LightningClient _client;

  // Design Pattern (Singleton)
  public static LnrpcConnection GetInstance()
  {
    if (_instance == null)
      _instance = new LnrpcConnection();

    return _instance;
  }

  private LnrpcConnection()
  {
    CreateConnection();
  }

  private Task AddMacaroon(AuthInterceptorContext context, Metadata metadata)
  {
    metadata.Add(new Metadata.Entry("macaroon", GetMacaroon()));
    return Task.CompletedTask;
  }

  private string GetMacaroon()
  {
    string diretoryMacaroon = "/home/yatogamidev/.lnd/data/chain/bitcoin/testnet/admin.macaroon";
    byte[] macaroonBytes = File.ReadAllBytes(diretoryMacaroon);
    var macaroon = BitConverter.ToString(macaroonBytes).Replace("-", "");
    return macaroon;
  }

  private void CreateConnection()
  {
    string channelHost = "https://localhost:10009";
    string diretoryCert = "/home/yatogamidev/.lnd/tls.cert";

    var rawCert = File.ReadAllBytes(diretoryCert);
    var x509Cert = new X509Certificate2(rawCert);

    var httpClientHandler = new HttpClientHandler
    {
      ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors)
          => x509Cert.Equals(cert)
    };

    var credentials = ChannelCredentials.Create(new SslCredentials(), CallCredentials.FromInterceptor(AddMacaroon));

    var channel = GrpcChannel.ForAddress(channelHost, new GrpcChannelOptions
    {
      HttpHandler = httpClientHandler,
      Credentials = credentials
    });

    _client = new Lightning.LightningClient(channel);
  }

  public Lightning.LightningClient GetConnection()
  {
    return _client;
  }
}
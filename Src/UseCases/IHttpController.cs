using Protocol;
namespace Controller
{
  public interface IHttpController<T>
  {
    public Task<HttpResponsePacket> Execute(HttpRequestPacket<T> request);
  }
  public interface IHttpController
  {
    public Task<HttpResponsePacket> Execute(HttpRequestNoPacket request);
  }
}

namespace Protocol
{
  public class HttpRequestPacket<T>
  {
    public T Data { get; set; }
    public string UserId { get; set; }

    public HttpRequestPacket() {}
    public HttpRequestPacket(string userId) {
      UserId = userId;
    }
  }

  public class HttpRequestNoPacket
  {
    public string UserId { get; set; }

    public HttpRequestNoPacket() {}
    public HttpRequestNoPacket(string userId) {
      UserId = userId;
    }
  }
}
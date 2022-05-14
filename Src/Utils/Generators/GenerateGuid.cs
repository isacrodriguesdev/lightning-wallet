
public static class GenerateGuid
{

  public static string Handle()
  {
    Guid uuid = Guid.NewGuid();
    return uuid.ToString();
  }
}
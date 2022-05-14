using System.Text.Json;

public static class DeepCopyObject
{
  public static T DeepCopy<T>(this T self)
  {
    var serialized = JsonSerializer.Serialize(self);
    return JsonSerializer.Deserialize<T>(serialized);
  }
}
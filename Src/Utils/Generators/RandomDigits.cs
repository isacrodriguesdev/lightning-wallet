
public class RandomDigits
{

  public static int Handle(int length)
  {
    Random rd = new Random();
    string s = String.Empty;
    for (var i = 0; i < length; i++) { s = String.Concat(s, rd.Next(0, 9).ToString()); }
    return Convert.ToInt32(s);
  }
}
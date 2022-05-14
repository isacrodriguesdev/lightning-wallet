namespace Helper
{
  public interface IValidatorControllerAdapter
  {
    string Validate<T>(T validateObj);
  }
}
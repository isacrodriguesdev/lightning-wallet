

public interface IObserverPayment
{
  Task Execute(ISubjectPayment subject);
}

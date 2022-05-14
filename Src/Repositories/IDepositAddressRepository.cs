using Model;

public interface IDepositAddressRepository {
  Task Create(DepositAddress depositAddress);
  Task<DepositAddress> FindByAddress(string address);
}
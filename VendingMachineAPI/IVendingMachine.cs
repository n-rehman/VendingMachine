using static VendingMachineAPI.CommonUtility;

namespace VendingMachineAPI
{
	public interface IVendingMachine
	{
		string getCurrentDisplay();
		string InsertCoin(ICoin c);
		string Dispense(productType product);
		string ReturnCoins();
	}
}
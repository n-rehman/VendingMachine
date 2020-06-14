using static VendingMachineAPI.CommonUtility;

namespace VendingMachineAPI
{
	public interface ICoin
	{
		
		enumCoinType getValue(out decimal value);
	}
}
using static VendingMachineAPI.CommonUtility;

namespace VendingMachineAPI
{
	public class Coin : ICoin
	{
		private decimal _weight;
		public Coin (decimal weight)
		{
			_weight = weight;
		}
		public enumCoinType getValue(out decimal value)
		{
			decimal monetaryValue = 0m;
			enumCoinType type = enumCoinType.Dime;
			switch(_weight)
			{
				case 5.000m:
					monetaryValue = 0.05m;
					type = enumCoinType.Nickel;
					break;
				case 2.268m:
					monetaryValue = 0.10m;
					type = enumCoinType.Dime;
					break;
				case 5.670m:
					monetaryValue = 0.25m;
					type = enumCoinType.Quarter;
					break;
			}
			value = monetaryValue;
			return type;
		}
	}
}
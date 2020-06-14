using System;
using System.Text;

namespace VendingMachineAPI
{
	public class VendingMachine : IVendingMachine
	{
		private const string INSERT_COIN_MSG = "INSERT COINS";
		private const string EXACT_CHANGE_ONLY_MSG = "EXACT CHANGE ONLY";
		private decimal _currentCoinsAmount = 0;
		//coins stock/change
		int _quarter_count = 0;
		int _dime_count = 0;
		int _nickel_count = 0;
		int _invalid_count = 0;
		//product price
		decimal _colaPrice =0;
		decimal _chipsPrice =0;
		decimal _candyPrice = 0;
		//product stock

		int _cola_stock = 20;
		int _chips_stock = 30;
		int _candy_stock = 30;
		
		//three kinds of coins
		private static decimal[] CoinArr = new decimal[] { 0.25M, 0.10M, 0.05M };
		//numbers of three kinds of coins to return
		private static int[] CoinCountList = null;

		public VendingMachine()
		{
			_quarter_count = 8;//$2
			_dime_count = 50;//$5
			_nickel_count = 20;//$1

			_colaPrice = 1.0m;
			_chipsPrice = 0.5m;
			_candyPrice = 0.65m;


			_cola_stock = 20;
			_chips_stock = 50;
			_candy_stock = 100;
		}
		#region test/replinishment
		public void setColaStock(int stock)
		{
			_cola_stock = stock;
		}
		public void setChipsStock(int stock)
		{
			_chips_stock = stock;
		}
		public void setCandyStock(int stock)
		{
			_candy_stock = stock;
		}
		public void replenishQuarter(int qty, bool setEmpty =false)
		{
			if (setEmpty)
			{
				_quarter_count = 0;
				return;
			}
				
			_quarter_count += qty;
		}
		public void replenishDime(int qty, bool setEmpty = false)
		{
			if (setEmpty)
			{
				_dime_count = 0;
				return;
			}

			_dime_count += qty;
		}
		public void replenishNickel(int qty, bool setEmpty = false)
		{
			if (setEmpty)
			{
				_nickel_count = 0;
				return;
			}

			_nickel_count += qty;
		}
		#endregion
		public string getCurrentDisplay()
		{
			if (_currentCoinsAmount == 0)
			{
				if(checkChange())
					return INSERT_COIN_MSG;
				else
					return EXACT_CHANGE_ONLY_MSG;
			}
			else
				return _currentCoinsAmount.ToString("#0.00");
		}
		private bool checkChange()
		{
			bool hasChange = true;
			// if we have change of 0.15p its enough due to combination of coins we using
			if (_dime_count < 1 && _nickel_count < 2)
				hasChange = false;
			else if (_nickel_count < 3)
				hasChange = false;

			return hasChange;
		}
		public string InsertCoin(ICoin coin)
		{
			decimal monetaryValue = 0;
			var coinType = coin.getValue(out monetaryValue);
			if (monetaryValue > 0)
			{
				_currentCoinsAmount += monetaryValue;
				incrementCoinCounter(coinType);

			}
			else
				_invalid_count++;

			return getCurrentDisplay();

		}

		private void incrementCoinCounter(CommonUtility.enumCoinType coinType)
		{
			switch (coinType)
			{
				case CommonUtility.enumCoinType.Dime:
					_dime_count++;
					break;
				case CommonUtility.enumCoinType.Quarter:
					_quarter_count++;
					break;
				case CommonUtility.enumCoinType.Nickel:
					_nickel_count++;
					break;
			}
		}

		public string Dispense(CommonUtility.productType product)
		{
			string _dispenseMsg = string.Empty;
			if (isInStock(product))
			{
				var productPrice = getProductPrice(product);
				if (_currentCoinsAmount >= productPrice)
				{
					_currentCoinsAmount -= productPrice;
					updateProductStock(product);

					_dispenseMsg = "THANK YOU";
					//_dispenseMsg += returnCoins(_currentCoinsAmount);
				}
				else if (_currentCoinsAmount < productPrice)
					_dispenseMsg = $"PRICE {productPrice}, Current Amount {_currentCoinsAmount}";
			}
			else
				_dispenseMsg = "SOLD OUT";
			return _dispenseMsg;

		}

		private void updateProductStock(CommonUtility.productType product)
		{
			if (product == CommonUtility.productType.Cola)
				_cola_stock -= 1;
			else if (product == CommonUtility.productType.Candy)
				_candy_stock -= 1;
			else if (product == CommonUtility.productType.Chips)
				_chips_stock -= 1;
		}

		public string ReturnCoins()
		{
			return returnCoins(_currentCoinsAmount);

		}
		// after purchase, return changes 
		private string returnCoins(decimal changes)
		{
			int change_quarter_count = 0;
			int change_dime_count = 0;
			int change_nickel_count = 0;

			decimal currentCoin = changes;
			CoinCountList = new int[] { 0, 0, 0 };
			int tmpCoin = Convert.ToInt32(currentCoin * 100);
			while (tmpCoin > 0)
			{
				for (int i = 0; i < CoinArr.Length; i++)
				{
					var currentCoinIn100Base = Convert.ToInt32(CoinArr[i] * 100);
					if (tmpCoin >= currentCoinIn100Base)
					{
						int result = tmpCoin / currentCoinIn100Base;
						CoinCountList[i] = result;
						tmpCoin = tmpCoin % currentCoinIn100Base;
						break;
					}
				}
			}

			int[] moneyCount = CoinCountList;

			change_quarter_count = moneyCount[0];
			change_dime_count = moneyCount[1];
			change_nickel_count = moneyCount[2];

			_quarter_count -= change_quarter_count;
			_dime_count -= change_dime_count;
			_nickel_count -= change_nickel_count;

			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"Quarter Change: {change_quarter_count.ToString()}");
			sb.AppendLine($"Dime Change: {change_dime_count.ToString()}");
			sb.AppendLine($"Nickel Change: {change_nickel_count.ToString().ToString()}");
			
			_currentCoinsAmount = 0m;
			sb.AppendLine($"Current Amount: {_currentCoinsAmount.ToString("#0.00")}");
			sb.Append(getCurrentDisplay());

			return sb.ToString();
		}
		private decimal getProductPrice(CommonUtility.productType product)
		{
			var productPrice = 0m;
			if (product == CommonUtility.productType.Cola)
				productPrice = _colaPrice;
			else if (product == CommonUtility.productType.Chips)
				productPrice = _chipsPrice;
			else if (product == CommonUtility.productType.Candy)
				productPrice = _candyPrice;

			return productPrice;

		}
		private bool isInStock(CommonUtility.productType product)
		{
			var inStock = false;
			if (product == CommonUtility.productType.Cola && _cola_stock > 0)
				inStock = true;
			else if (product == CommonUtility.productType.Chips && _chips_stock > 0)
				inStock = true;
			else if (product == CommonUtility.productType.Candy && _candy_stock > 0)
				inStock = true;

			return inStock;

		}
	}
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachineAPI;

namespace VendingMachineKata
{
	[TestClass]
	public class VendingMachineUnitTests
	{
		[TestMethod]
		public void Test_NoCoins_InsertMessage()
		{
			//Arrange
			IVendingMachine vm = new VendingMachine();

			//Act
			string msg = vm.InsertCoin(new Coin(0));

			//Assert
			Assert.AreEqual("INSERT COINS", msg);

		}
		[TestMethod]
		public void Test_NoCoinsNoChange_InsertMessage()
		{
			//Arrange
			VendingMachine vmInstance = new VendingMachine();
			vmInstance.replenishQuarter(0, true);
			vmInstance.replenishDime(0, true);
			vmInstance.replenishNickel(0, true);

			IVendingMachine vm = vmInstance;

			//Act
			string msg = vm.InsertCoin(new Coin(0));

			//Assert
			Assert.AreEqual("EXACT CHANGE ONLY", msg);

		}
		[TestMethod]
		public void Test_InsertQuarterCoin_DisplayQuarter()
		{
			//Arrange
			IVendingMachine vm = new VendingMachine();

			//Act
			ICoin c = new Coin(5.670m);
			
			string msg = vm.InsertCoin(c);

			//Assert
			Assert.AreEqual("0.25", msg);

		}
		[TestMethod]
		public void Test_InsertDimeCoin_DisplayOneDimeAmount()
		{
			//Arrange
			IVendingMachine vm = new VendingMachine();

			//Act
			ICoin c = new Coin(2.268m);

			string msg = vm.InsertCoin(c);

			//Assert
			Assert.AreEqual("0.10", msg);

		}
		[TestMethod]
		public void Test_InsertNickelCoin_DisplayOneNickelAmount()
		{
			//Arrange
			IVendingMachine vm = new VendingMachine();

			//Act
			ICoin c = new Coin(5.000m);

			string msg = vm.InsertCoin(c);

			//Assert
			Assert.AreEqual("0.05", msg);

		}
		[TestMethod]
		public void Test_InsertMultiValidCoin_DisplayCorrectAmount()
		{
			//Arrange
			IVendingMachine vm = new VendingMachine();

			//Act
			ICoin c = new Coin(5.000m);
			string msg = vm.InsertCoin(c);
			c = new Coin(2.268m);
			 msg = vm.InsertCoin(c);
			c = new Coin(5.670m);
			msg = vm.InsertCoin(c);


			//Assert
			Assert.AreEqual("0.40", msg);

		}
		[TestMethod]
		public void Test_InsertPennieCoin_DisplayInsertMessage()
		{
			//Arrange
			IVendingMachine vm = new VendingMachine();

			//Act
			ICoin c = new Coin(0m);

			string msg = vm.InsertCoin(c);

			//Assert
			Assert.AreEqual("INSERT COINS", msg);

		}
		[TestMethod]
		public void Test_InsertExactCoin_DispsenseCola()
		{
			//Arrange
			IVendingMachine vm = new VendingMachine();
			
			//Act
			ICoin c = new Coin(5.670m);
			
			string msg = vm.InsertCoin(c);
			Assert.AreEqual("0.25", msg);

			 msg = vm.InsertCoin(c);
			Assert.AreEqual("0.50", msg);


			msg = vm.InsertCoin(c);
			Assert.AreEqual("0.75", msg);


			msg = vm.InsertCoin(c);
			Assert.AreEqual("1.00", msg);

			msg = vm.Dispense(CommonUtility.productType.Cola);
			//Assert
			Assert.AreEqual("THANK YOU", msg);

		}
		[TestMethod]
		public void Test_InsertExactColaCoin_OutOfStock()
		{
			//Arrange
			VendingMachine vmInstance = new VendingMachine();
			vmInstance.setColaStock(0);
			IVendingMachine vm = vmInstance;

			//Act
			ICoin c = new Coin(5.670m);

			string msg = vm.InsertCoin(c);
			Assert.AreEqual("0.25", msg);

			msg = vm.InsertCoin(c);
			Assert.AreEqual("0.50", msg);


			msg = vm.InsertCoin(c);
			Assert.AreEqual("0.75", msg);


			msg = vm.InsertCoin(c);
			Assert.AreEqual("1.00", msg);

			msg = vm.Dispense(CommonUtility.productType.Cola);
			//Assert
			Assert.AreEqual("SOLD OUT", msg);

		}
		[TestMethod]
		public void Test_InsertCoins_ReturnAllChange()
		{
			//Arrange
			VendingMachine vmInstance = new VendingMachine();
			IVendingMachine vm = vmInstance;

			//Act
			ICoin c = new Coin(5.670m);

			string msg = vm.InsertCoin(c);
			Assert.AreEqual("0.25", msg);

			msg = vm.InsertCoin(c);
			Assert.AreEqual("0.50", msg);


			msg = vm.InsertCoin(c);
			Assert.AreEqual("0.75", msg);


			msg = vm.InsertCoin(c);
			Assert.AreEqual("1.00", msg);

			msg = vm.ReturnCoins();
			//Assert
			Assert.AreEqual("Quarter Change: 4\r\nDime Change: 0\r\nNickel Change: 0\r\nCurrent Amount: 0.00\r\nINSERT COINS", msg);

		}
		[TestMethod]
		public void Test_Buy1Candy_ExpectedChange15Pence()
		{
			//Arrange
			VendingMachine vmInstance = new VendingMachine();
			IVendingMachine vm = vmInstance;

			//Act
			ICoin c = new Coin(5.670m);

			string msg = vm.InsertCoin(c);
			Assert.AreEqual("0.25", msg);

			msg = vm.InsertCoin(c);
			Assert.AreEqual("0.50", msg);


			msg = vm.InsertCoin(c);
			Assert.AreEqual("0.75", msg);

			msg = vm.Dispense(CommonUtility.productType.Candy);
			//Assert
			Assert.AreEqual("THANK YOU", msg);

			msg = vm.ReturnCoins();
			//Assert
			Assert.AreEqual("Quarter Change: 0\r\nDime Change: 1\r\nNickel Change: 0\r\nCurrent Amount: 0.00\r\nINSERT COINS", msg);

		}
	}
}

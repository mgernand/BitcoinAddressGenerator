using System;

namespace BitcoinAddressGenerator.Console
{
	using System.Diagnostics;
	using Core;
	using Console = System.Console;

	class Program
	{
		static void Main(string[] args)
		{
			BitcoinAddressGenerator addressGenerator = new BitcoinAddressGenerator();
			AddressInfo addressInfo = addressGenerator.GenerateAddress(CoinType.Bitcoin);

			Console.WriteLine(addressInfo.Address);
			Process.Start("http://www.blockexplorer.com/address/" + addressInfo.Address);
			Console.ReadKey(true);
		}
	}
}

namespace BitcoinAddressGenerator.Console
{
	using System;
	using System.Linq;
	using System.Threading;
	using Core;
	using Console = System.Console;

	internal static class Program
	{
		private static void Main(string[] args)
		{
			CoinType coinType = GetCoinType(args);

			Console.WindowWidth = 150;

			Console.WriteLine("Bitcoin Address Generator");
			Console.WriteLine("=========================");
			Console.WriteLine();

			GenerateAddress(coinType);

			Console.WriteLine("Press any key to quit...");
			Console.ReadKey(true);
		}

		private static void GenerateAddress(CoinType coinType)
		{
			BitcoinAddressGenerator addressGenerator = new BitcoinAddressGenerator();
			AddressInfo addressInfo = addressGenerator.GenerateAddress(coinType);

			Console.Write("Generating new address");
			FakeProgress();
			Console.WriteLine();

			Console.WriteLine($"Private Key (hex): {addressInfo.PrivateKeyHex}");
			Console.WriteLine($"Private Key (wif): {addressInfo.PrivateKeyWif}");
			Console.WriteLine($"Public Key  (hex): {addressInfo.PublicKeyHex}");
			Console.WriteLine($"Public Key (hash): {addressInfo.PublicKeyHash}");
			Console.WriteLine();
			Console.WriteLine("=========================================================");
			Console.WriteLine($"| Address ({addressInfo.CoinType:G}): {addressInfo.Address} |");
			Console.WriteLine("=========================================================");
			Console.WriteLine();
		}

		private static void FakeProgress()
		{
			for (int i = 0; i < 10; i++)
			{
				Thread.Sleep(25);
				Console.Write(".");
			}

			Console.WriteLine(" complete!");
		}


		private static CoinType GetCoinType(string[] args)
		{
			CoinType coinType = CoinType.Bitcoin;

			if (args.Any())
			{
				if (args.First() == "--testnet")
				{
					coinType = CoinType.Testnet;
				}
			}

			return coinType;
		}
	}
}

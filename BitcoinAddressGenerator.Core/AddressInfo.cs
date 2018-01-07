namespace BitcoinAddressGenerator.Core
{
	using JetBrains.Annotations;

	[PublicAPI]
	public class AddressInfo
	{
		public string PrivateKeyHex { get; set; }

		public string PrivateKeyWif { get; set; }

		public string PublicKeyHex { get; set; }

		public string PublicKeyHash { get; set; }

		public string Address { get; set; }

		public CoinType CoinType { get; set; }
	}
}
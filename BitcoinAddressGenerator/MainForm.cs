namespace BitcoinAddressGenerator
{
	using System;
	using System.Diagnostics;
	using System.Drawing;
	using System.Windows.Forms;
	using Core;

	public partial class MainForm : Form
	{
		public MainForm()
		{
			this.InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			this.comboBoxCoinTypeGenerate.SelectedIndex = 0;
		}

		private void buttonGenerateAddress_Click(object sender, EventArgs e)
		{
			BitcoinAddressGenerator addressGenerator = new BitcoinAddressGenerator();
			CoinType coinType = (CoinType) Enum.Parse(typeof(CoinType), (string)this.comboBoxCoinTypeGenerate.SelectedItem);
			AddressInfo addressInfo = addressGenerator.GenerateAddress(coinType);

			this.textBoxPrivateKeyHex.Text = addressInfo.PrivateKeyHex;
			this.textBoxPrivateKeyWif.Text = addressInfo.PrivateKeyWif;
			this.textBoxPublicKeyHex.Text = addressInfo.PublicKeyHex;
			this.textBoxPublicKeyHash.Text = addressInfo.PublicKeyHash;
			this.textBoxAddress.Text = addressInfo.Address;
			this.labelAddress.Text = $@"Address ({coinType:G})";

			this.comboBoxCoinTypeBlockExplorer.SelectedIndex = this.comboBoxCoinTypeGenerate.SelectedIndex;
			this.labelStatus.ForeColor = Color.Green;
			this.labelStatus.Text = @"Address generation successful!";
		}

		private void buttonBlockExplorer_Click(object sender, EventArgs e)
		{
			CoinType coinType = (CoinType)Enum.Parse(typeof(CoinType), (string)this.comboBoxCoinTypeBlockExplorer.SelectedItem);

			string baseUrl;

			switch (coinType)
			{
				case CoinType.Bitcoin:
					baseUrl = "http://www.blockexplorer.com/address";
					break;
				case CoinType.Testnet:
					baseUrl = "http://www.blockexplorer.com/testnet/address";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			this.labelStatus.ForeColor = Color.DarkOrange;
			this.labelStatus.Text = @"Opening address in block explorer...";
			this.timer.Start();

			Process.Start($"{baseUrl}/{this.textBoxAddress.Text}");

			this.timer.Start();
		}

		private void buttonCopyAddress_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(this.textBoxAddress.Text);

			this.labelStatus.ForeColor = Color.Green;
			this.labelStatus.Text = @"Address copied to clipboard!";
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			this.timer.Stop();

			this.labelStatus.ForeColor = Color.Green;
			this.labelStatus.Text = @"Address showing in block explorer!";
		}
	}
}
